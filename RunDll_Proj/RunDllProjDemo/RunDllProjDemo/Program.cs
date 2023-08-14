using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region (3) - (5) DefaultProgram
// (3) Config
builder.SetConfig();
// (4) Service
builder.SetService();
// (5) Cors
builder.SetCors();
#endregion

// (6) HttpContextAccessor 
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// (7) Use Swagger from DefaultProgram
app.AppBuilder();

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// (8) Use Cors
app.UseCors();

// (9) Use Authentication
app.UseAuthentication();// 先驗證，驗證沒過再嘗試授權

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
