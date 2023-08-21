using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region (J-3) - (J-5)、(Ser-2) DefaultProgram
// (J-3) Config
builder.SetConfig();
// (J-4) Service
builder.SetService();
// (J-5) Cors
builder.SetCors();
// (Ser-2) Scoped
builder.SetScoped();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// (sw-3) DefaultProgram
app.AppBuilder();

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// (J-6) Use Cors
app.UseCors();

// (J-7) Use Authentication
app.UseAuthentication();// 先驗證，驗證沒過再嘗試授權

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
