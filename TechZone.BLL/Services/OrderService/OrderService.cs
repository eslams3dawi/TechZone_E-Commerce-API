using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.API.Middleware.CustomExceptions;
using TechZone.BLL.DTOs.OrderDTOs;
using TechZone.BLL.DTOs.PaymentDTOs;
using TechZone.BLL.Services.Email;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Enums.Order;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.OrderRepo;
using TechZone.DAL.Repository.ShoppingCartRepo;

namespace TechZone.BLL.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public OrderService(IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
        }
        public async Task CancelOrder(int orderId)
        {
            var order = await _orderRepository.GetById(orderId);
            if (order == null)
                throw new BadRequestException($"Order with Id: {orderId} not found");

            await _orderRepository.Delete(order);
            //delete also orderDetails within this header
        }

        public async Task<int> CreateOrder(OrderAddDTO orderAddDTO)
        {
            var existingOrder = await _orderRepository
                .GetFirstOrDefault(o => o.ApplicationUserId == orderAddDTO.ApplicationUserId
                && o.OrderStatus == OrderStatus.Pending, include: q => q.Include(o => o.OrderDetails));

            var cartItems = await _shoppingCartRepository.GetByUserId(orderAddDTO.ApplicationUserId);//new: not be added yet
            var orderDetails = cartItems.Select(item => new OrderDetail()
            {
                ProductId = item.ProductId,
                Count = item.Count,
                Price = item.Price,
               Brand = item.Brand 
            }).ToList();

            if (existingOrder != null) //if the order header is exist
            {
                var existingProductIds = existingOrder.OrderDetails
                    .Select(o => o.ProductId)
                    .ToHashSet();

                var orderDetailsNotExistInOldOrder = orderDetails
                    .Where(od => !existingProductIds.Contains(od.ProductId))
                    .ToList();

                foreach (var item in orderDetailsNotExistInOldOrder)
                {
                    existingOrder.OrderDetails.Add(item);
                }

                var orderIdOfExisting = existingOrder.Id;
                await _orderRepository.Update(existingOrder);

                var subject = "Your order is created";
                var body = "<h2>The products are added to your order</h2><p>Your order is pending, please wait until be reviewed</p>";

                var user = await _userManager.FindByIdAsync(orderAddDTO.ApplicationUserId);
                var userEmail = user.Email;

                await _emailService.SendEmailAsync(userEmail, subject, body);

                return orderIdOfExisting;
            }
            else
            {
                var orderHeader = _mapper.Map<OrderHeader>(orderAddDTO);
                orderHeader.OrderDetails = orderDetails;

                var orderId = await _orderRepository.AddOrderAndReturnId(orderHeader);

                var subject = "Your order is created";
                var body = "<h2>Thank you for your purchase</h2><p>Your order is pending, please wait until be reviewed</p>";

                var user = await _userManager.FindByIdAsync(orderAddDTO.ApplicationUserId);
                var userEmail = user.Email;

                await _emailService.SendEmailAsync(userEmail, subject, body);
                return orderId;
            }
        }

        public async Task<Result<IEnumerable<OrderDetailReadDTO>>> GetOrderDetails(int orderHeaderId)
        {
            var orderDetails = await _orderRepository.GetOrderDetails(orderHeaderId);
            if (orderDetails == null)
                return Result<IEnumerable<OrderDetailReadDTO>>.Failure($"Order items not found for order: {orderHeaderId}", null, ActionCode.NotFound);

            var orderDetailDTOs = _mapper.Map<IEnumerable<OrderDetailReadDTO>>(orderDetails);
            return Result<IEnumerable<OrderDetailReadDTO>>.Success(orderDetailDTOs);
        }

        public async Task<Result<IEnumerable<OrderHeaderReadDTO>>> GetOrders()
        {
            var orders = await _orderRepository.GetAll();

            var orderDTOs = _mapper.Map<IEnumerable<OrderHeaderReadDTO>>(orders);

            return Result<IEnumerable<OrderHeaderReadDTO>>.Success(orderDTOs);
        }

        public async Task<Result<string>> GetOrderHeader(int orderId)
        {
            var orderHeader = await _orderRepository.GetById(orderId);

            var status = orderHeader.OrderStatus.ToString();
            return Result<string>.Success(status);
        }

        public async Task UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            var order = await _orderRepository.GetById(orderId);
            if (order.OrderStatus == orderStatus)
                return;

            order.OrderStatus = orderStatus;
            await _orderRepository.Update(order);

            var subject = $"Your Order Has Been {orderStatus.ToString()}!";

            string body = "";
            switch (orderStatus)
            {
                case OrderStatus.Approved:
                body = @"<h2>Your Order Has Been Approved!</h2>
<p>Thank you for your purchase. 
Your order is now confirmed and will be processed shortly.<br/>
We will notify you once it has been shipped.</p>";
                    break;
                case OrderStatus.Shipped:
                    body = @"<h2>Your Order Has Been Shipped!</h2>
<p>Your package is on the way! <br/>
Thank you for shopping with us!</p>";
                    break;
                case OrderStatus.Delivered:
                    body = @"<h2>Your Order Has Been Delivered!</h2>
<p>We hope you're enjoying your new purchase. If you have any questions or feedback, feel free to contact our support team.<br/>
Thank you again for choosing us.</p>";
                    break;
                case OrderStatus.Cancelled:
                    body = @"<h2>Your Order Has Been Cancelled!</h2>
<p>We're sorry to inform you that your order was cancelled. If this was unexpected or you'd like to place a new order, please contact our support.</p>";
                    break;
            }

            var orderHeader = await _orderRepository.GetById(orderId);
            if (orderHeader == null)
                throw new NullReferenceException();

            var user = await _userManager.FindByIdAsync(orderHeader.ApplicationUserId);
            var userEmail = user.Email;

            await _emailService.SendEmailAsync(userEmail, subject, body);
        }

        public async Task UpdateShippingDate(int orderId, DateTime shippingDate)
        {
            var order = await _orderRepository.GetById(orderId);
            order.ShippingDate = shippingDate;

            await _orderRepository.Update(order);
        }
    }
}
