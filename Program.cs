using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using vizar.repositiory;
using vizar.Settings;

var builder = WebApplication.CreateBuilder(args);


//Serelize Settings
BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));

// Add services to the container.

//Local Host

/*
builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>{
    var settings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
});
*/


builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>{
    var settings = MongoClientSettings.FromConnectionString("mongodb+srv://abdenourzermat:NfmShbcSa4vx3g5J@vizar.lrn76gc.mongodb.net/?retryWrites=true&w=majority");
    return new MongoClient(settings);
});

//Users Singleton
builder.Services.AddSingleton<IusersRepository,MongoDBUsersRepository>();
//products Singleton
builder.Services.AddSingleton<IProductsRepository,MongoDBProductsRepository>();

//Images Repository Singleton
builder.Services.AddSingleton<IImagesRepository,FTPImageRepository>();
//Models Repository Singleton
builder.Services.AddSingleton<IModelsRepository,FTPModelsRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();

provider.Mappings.Add(new KeyValuePair<string, string>(".fbx", "application/model"));

app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = provider
});

app.UseStaticFiles();
// Configure the HTTP request pipeline.

/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

//Build the swagger ui for linkers
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
