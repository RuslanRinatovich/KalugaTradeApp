using WepTradeApp.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/users", () =>
{
    TradeContext context = new TradeContext();
    
    return context.Users.ToList();
    
})
.WithName("GetAllUsers")
.WithOpenApi();

app.MapGet("/users/{id}", (string id) =>
{
    TradeContext context = new TradeContext();
     User user = context.Users.FirstOrDefault(x => x.Username == id);
     if (user == null)
        return Results.NotFound("User not found!");
    return Results.Ok(user);
})
.WithName("GetUserWithUsername")
.WithOpenApi();


app.MapGet("/products", () =>
{
    TradeContext context = new TradeContext();
    
    return context.Products.ToList();
    
})
.WithName("GetAllProducts")
.WithOpenApi();

app.MapGet("/products/{id}", (string id) =>
{
    TradeContext context = new TradeContext();
     Product product = context.Products.FirstOrDefault(x => x.Id == id);
     if (product == null)
        return Results.NotFound("Product not found!");
    return Results.Ok(product);
})
.WithName("GetProductWithId")
.WithOpenApi();

app.MapPost("/product",async([FromForm]ProductRequest model)=>{
 if(model.Photo == null)
 return Results.BadRequest("Прикрепите фото!");
 var product = new Product(model);
 product.Photo = await SaveImage(model.Photo);
 context.Product.Add(product);
 context.SaveChanges();
 return Results.Ok("Товар добавлен!");
})
.DisableAntiforgery()
.Accepts<ProductRequest>("multipart/form-data")
.WithName("CreateProduct")
.WithOpenApi();



async Task<byte[]> SaveImage(IFormFile  file){
    using (var stream = new MemoryStream()){
        file.CopyToAsync(stream).Wait();
        return stream.ToArray();
    }
}




app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
