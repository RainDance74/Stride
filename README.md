# Welcome to Stride! 🥾

Stride is a todo application built using ASP.NET Core for the backend and React for the frontend. To explore the entire repository and potentially build it yourself, head over to this repository: [github.com/nayvok-raindance/Stride](https://github.com/nayvok-raindance/Stride).

# How do I build it? 💻

Before of building it, you should first install certificates on your local machine. Why? [Because](https://github.com/microsoft/dotnet-framework-docker/issues/799).

**With Visual Studio**

In case if you have Visual Studio installed, you can just open it and install certificate from it. If you did it, go to **Launching Docker Compose** section.

**Without Visual Studio**

You can configure certificates using `dotnet dev-certs tool`. To do it, please follow [official documentation](https://learn.microsoft.com/ru-ru/aspnet/core/security/docker-https?view=aspnetcore-8.0). You'll have to add required params, like `ASPNETCORE_Kestrel__Certificates__Default__Password` and `ASPNETCORE_Kestrel__Certificates__Default__Path` in your `docker compose up` command.

**Launching Docker Compose**

The easiest way to build Stride is with Docker. Simply run `docker compose up` to build the application. 

**Run in terminal:**
    docker compose up

Your app will be accessed from ports 8080 for http and 8081 for https.

**Without Docker:**

If you don't have Docker installed, you can still run Stride. Set your connection string to your PostgreSQL database as an environment variable named `ConnectionStrings__DefaultConnection`, either for your user or your entire computer. Then, with .NET 8.0 installed, run the application from the root repository directory using `dotnet run src/Web/Web.csproj`. The exposed port will be shown in the output, starting with `info: Microsoft.Hosting.Lifetime[14]`.

**Example output:**

    info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5432

That's all you need to get started with Stride!

# Description 🗒️

## Stride under the hood: 🚘

- **Built with muscle:** Uncle Bob's Clean Architecture principles keep things organized and flexible, like a well-equipped gym where every piece of equipment has its place and purpose.
- **Monolithic but mighty:** Sure, it's not a microservices dance party, but Stride's one-piece design delivers seamless performance and consistency.
- **Tech symphony:** NSwag conducts the API orchestra, PostgreSQL and Asp.Identity keep the data vault secure, and MediatoR acts as the baton, ensuring smooth communication between frontend and backend.
