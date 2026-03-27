var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<WarehouseQL.GraphQL.Query>()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGraphQL("/graphql");

app.Run();
