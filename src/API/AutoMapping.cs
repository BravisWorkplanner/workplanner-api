using API.V1.Features.Expenses.Responses;
using API.V1.Features.Orders.Request;
using API.V1.Features.Orders.Response;
using API.V1.Features.Workers.Request;
using API.V1.Features.Workers.Response;
using AutoMapper;
using Domain.Entities;

namespace API
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            // orders
            CreateMap<Order, OrderGetResult>()
                .ForMember(x => x.OrderId, expression => expression.MapFrom(x => x.Id));

            CreateMap<Order, OrderListResult>()
                .ForMember(x => x.OrderId, expression => expression.MapFrom(x => x.Id));

            CreateMap<OrderUpdateRequest, Order>()
                .ForMember(x => x.ObjectNumber, expression => expression.Ignore())
                .ForMember(x => x.TimeRegistrations, expression => expression.Ignore())
                .ForMember(x => x.Expenses, expression => expression.Ignore())
                .ForMember(x => x.Id, expression => expression.MapFrom(x => x.OrderId));

            CreateMap<OrderCreateRequest, Order>().
                ForMember(x => x.Id, expression => expression.Ignore()).
                ForMember(x => x.TimeRegistrations, expression => expression.Ignore()).
                ForMember(x => x.Expenses, expression => expression.Ignore());

            // workers
            CreateMap<WorkerCreateRequest, Worker>()
                .ForMember(x => x.Id, expression => expression.Ignore());

            CreateMap<Worker, WorkerGetResult>()
                .ForMember(x => x.WorkerId, expression => expression.MapFrom(x => x.Id));

            CreateMap<Worker, WorkerListResult>()
                .ForMember(x => x.WorkerId, expression => expression.MapFrom(x => x.Id));

            // expenses
            CreateMap<Expense, ExpenseResult>();

            CreateMap<Expense, ExpenseListResult>()
                .ForMember(x => x.WorkerId, exp => exp.MapFrom(x => x.Worker.Id))
                .ForMember(x => x.OrderId, exp => exp.MapFrom(x => x.Order.Id));

            // time registrations
            CreateMap<TimeRegistration, TimeRegistrationResult>();
        }
    }
}