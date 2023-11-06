using System;
using System.Text.Json;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ProductService.Services.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory,
        IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.CustomerPublished:
                    addCustomer(message);
                    break;
                case EventType.TaskerPublished:
                    addTasker(message);
                    break;
                case EventType.CustomerUpdatePublished:
                    updateCustomer(message);
                    break;
                case EventType.TaskerUpdatePublished:
                    udpateTasker(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventModel>(notificationMessage);

            switch (eventType.Event)
            {
                case "Customer_Published":
                    Console.WriteLine("--> Customer Published Event Detected");
                    return EventType.CustomerPublished;
                case "Tasker_Published":
                    Console.WriteLine("--> Tasker Published Event Detected");
                    return EventType.TaskerPublished;
                case "Customer_Update_Published":
                    Console.WriteLine("--> Customer Update Published Event Detected");
                    return EventType.CustomerUpdatePublished;
                case "Tasker_Update_Published":
                    Console.WriteLine("--> Tasker Update Published Event Detected");
                    return EventType.TaskerUpdatePublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermind;
            }
        }

        private void addCustomer(string customerMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var customer = JsonSerializer.Deserialize<CustomerPublishedModel>(customerMessage);
                Console.WriteLine("Customer:" + customer.Id);
                try
                {
                    var customerEntity = _mapper.Map<Customer>(customer);
                    Console.WriteLine("CustomerEntity:" + customerEntity.Id);
                    if (!repo.ExternalCustomerExists(customerEntity.ExternalId))
                    {
                        repo.CreateCustomer(customerEntity);
                        Console.WriteLine($"--> Customer Created: {customerEntity.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"--> Customer already exists: {customerEntity.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Customer to database: {ex.Message}");
                }
            }
        }

        private void updateCustomer(string customerMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var customer = JsonSerializer.Deserialize<CustomerPublishedModel>(customerMessage);
                Console.WriteLine("Customer:" + customer.Id);
                try
                {
                    var customerEntity = repo.GetCustomerByExternalId(customer.Id);

                    if (customerEntity != null)
                    {
                        customerEntity.Name = customer.Name;
                        customerEntity.Address = customer.Address;
                        customerEntity.Email = customer.Email;
                        customerEntity.Status = customer.Status;
                        repo.UpdateCustomer(customerEntity);
                        Console.WriteLine($"--> Customer Update: {customerEntity.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"--> Customer doesn't exists: {customerEntity.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Customer to database: {ex.Message}");
                }
            }
        }

        private void addTasker(string taskerMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var tasker = JsonSerializer.Deserialize<TaskerPublishedModel>(taskerMessage);

                try
                {
                    var taskerEntity = _mapper.Map<Tasker>(tasker);
                    if (!repo.ExternalTaskerExists(taskerEntity.ExternalId))
                    {
                        repo.CreateTasker(taskerEntity);
                        Console.WriteLine($"--> Tasker Created: {taskerEntity.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"--> Tasker already exists: {taskerEntity.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Tasker to database: {ex.Message}");
                }
            }
        }

        private void udpateTasker(string taskerMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var tasker = JsonSerializer.Deserialize<TaskerPublishedModel>(taskerMessage);
                try
                {
                    var taskerEntity = repo.GetTaskerByExternalId(tasker.Id);

                    if (taskerEntity != null)
                    {
                        taskerEntity.Name = tasker.Name;
                        taskerEntity.Address = tasker.Address;
                        taskerEntity.Email = tasker.Email;
                        taskerEntity.Status = tasker.Status;
                        taskerEntity.Identification = tasker.Identification;
                        repo.UpdateTasker(taskerEntity);
                        Console.WriteLine($"--> tasker Update: {taskerEntity.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"--> tasker doesn't exists: {taskerEntity.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Customer to database: {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        CustomerPublished,
        CustomerUpdatePublished,
        TaskerPublished,
        TaskerUpdatePublished,
        Undetermind
    }
}