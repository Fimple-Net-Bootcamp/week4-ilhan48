﻿using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetManagement.Contracts;
using PetManagement.Database.Context;

namespace PetManagement.Features.Users;

public static class GetUser
{
    public class Query : IRequest<UserResponse>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handle : IRequestHandler<Query, UserResponse>
    {
        private readonly PetManagementDbContext _context;

        public Handle(PetManagementDbContext context)
        {
            _context = context;
        }

        async Task<UserResponse> IRequestHandler<Query, UserResponse>.Handle(Query request, CancellationToken cancellationToken)
        {
            var userResponse = await _context
                .Users
                .Where(user => user.Id == request.Id)
                .Select(user => new UserResponse
                {
                   FirstName = user.FirstName,
                   LastName = user.LastName,
                   Email = user.Email,
                   UserName = user.UserName,
                   CreatedOnUtc = user.CreatedDate,


                    Pets = user.OwnedPets.Select(pet => new PetResponse
                    {
                        Name = pet.Name,
                        Type = pet.Type,
                        BirthDate = pet.BirthDate,
                        Color = pet.Color,
                        Gender = pet.Gender,

                    }).ToList()


                })
                .FirstOrDefaultAsync(cancellationToken);


            return userResponse;
        }
    }
}

public class GetUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/users/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetUser.Query { Id = id };

            var result = await sender.Send(query);

            return Results.Ok(result);
        });
    }
}
