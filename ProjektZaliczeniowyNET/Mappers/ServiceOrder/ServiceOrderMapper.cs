using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using ProjektZaliczeniowyNET.DTOs.Comment;
using ProjektZaliczeniowyNET.DTOs.ServiceOrderPart;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ProjektZaliczeniowyNET.Mappers
{
    public class ServiceOrderMapper : IServiceOrderMapper
    {
        public ServiceOrderDto ToDto(ServiceOrder order)
        {
            if (order == null) return null!;

            return new ServiceOrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Description = order.Description,
                Status = order.Status,
                StatusDisplay = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                StartedAt = order.StartedAt,
                CompletedAt = order.CompletedAt,
                EstimatedCompletionDate = order.EstimatedCompletionDate,
                EstimatedCost = order.EstimatedCost,
                FinalCost = order.FinalCost,
                TotalLaborCost = order.TotalLaborCost,
                TotalPartsCost = order.TotalPartsCost,
                TotalCost = order.TotalCost,
                CustomerComplaints = order.CustomerComplaints,
                InternalNotes = order.InternalNotes,
                IsWarrantyWork = order.IsWarrantyWork,

                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.FullName ?? "",
                CustomerEmail = order.Customer?.Email ?? "",
                CustomerPhone = order.Customer?.PhoneNumber ?? "",

                VehicleId = order.VehicleId,
                VehicleDisplayName = order.Vehicle?.DisplayName ?? "",
                VehicleLicensePlate = order.Vehicle?.LicensePlate ?? "",

                AssignedMechanicId = order.AssignedMechanicId,
                AssignedMechanicName = order.AssignedMechanic?.UserName ?? "",

                CreatedByUserId = order.CreatedByUserId,
                CreatedByUserName = order.CreatedByUser?.UserName ?? "",

                ServiceTasks = order.ServiceTasks.Select(st => new ServiceTaskDto
                {
                    Id = st.Id,
                    Description = st.Description,
                    TotalTaskCost = st.TotalTaskCost,
                    IsCompleted = st.IsCompleted,
                    // ... inne właściwości ServiceTaskDto jeśli są
                }).ToList(),

                Comments = order.Comments.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    AuthorName = c.Author?.UserName ?? ""
                }).ToList(),

                ServiceOrderParts = order.ServiceOrderParts.Select(p => new ServiceOrderPartDto
                {
                    Id = p.Id,
                    PartName = p.Part?.Name ?? "",
                    Quantity = p.Quantity,
                    Cost = p.Cost
                }).ToList(),

                TasksCount = order.ServiceTasks.Count,
                CompletedTasksCount = order.ServiceTasks.Count(st => st.IsCompleted),
                CommentsCount = order.Comments.Count
            };
        }

        public ServiceOrderListDto ToListDto(ServiceOrder order)
        {
            if (order == null) return null!;

            return new ServiceOrderListDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Description = order.Description,
                Status = order.Status,
                StatusDisplay = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                EstimatedCompletionDate = order.EstimatedCompletionDate,
                EstimatedCost = order.EstimatedCost,
                TotalCost = order.TotalCost,
                CustomerName = order.Customer?.FullName ?? "",
                VehicleDisplayName = order.Vehicle?.DisplayName ?? "",
                AssignedMechanicName = order.AssignedMechanic?.UserName ?? "",
                TasksCount = order.ServiceTasks.Count,
                CompletedTasksCount = order.ServiceTasks.Count(st => st.IsCompleted),
                IsWarrantyWork = order.IsWarrantyWork
            };
        }

        public IEnumerable<ServiceOrderListDto> ToListDto(IEnumerable<ServiceOrder> orders)
        {
            return orders.Select(ToListDto);
        }

        public ServiceOrderSelectDto ToSelectDto(ServiceOrder order)
        {
            if (order == null) return null!;

            return new ServiceOrderSelectDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Description = order.Description,
                Status = order.Status,
                CustomerName = order.Customer?.FullName ?? "",
                VehicleDisplayName = order.Vehicle?.DisplayName ?? ""
            };
        }

        public ServiceOrder ToEntity(ServiceOrderCreateDto dto, string createdByUserId)
        {
            if (dto == null) return null!;

            return new ServiceOrder
            {
                Description = dto.Description,
                EstimatedCompletionDate = dto.EstimatedCompletionDate,
                EstimatedCost = dto.EstimatedCost,
                CustomerComplaints = dto.CustomerComplaints,
                InternalNotes = dto.InternalNotes,
                IsWarrantyWork = dto.IsWarrantyWork,
                CustomerId = dto.CustomerId,
                VehicleId = dto.VehicleId,
                AssignedMechanicId = dto.AssignedMechanicId,
                CreatedByUserId = createdByUserId,
                CreatedAt = DateTime.UtcNow,
                Status = ServiceOrderStatus.Pending,
                FinalCost = 0m
            };
        }

        public void UpdateEntity(ServiceOrder entity, ServiceOrderUpdateDto dto)
        {
            if (entity == null || dto == null) return;

            entity.Description = dto.Description;
            entity.Status = dto.Status;
            entity.StartedAt = dto.StartedAt;
            entity.CompletedAt = dto.CompletedAt;
            entity.EstimatedCompletionDate = dto.EstimatedCompletionDate;
            entity.EstimatedCost = dto.EstimatedCost;
            entity.FinalCost = dto.FinalCost;
            entity.CustomerComplaints = dto.CustomerComplaints;
            entity.InternalNotes = dto.InternalNotes;
            entity.IsWarrantyWork = dto.IsWarrantyWork;
            entity.AssignedMechanicId = dto.AssignedMechanicId;
        }
    }
}
