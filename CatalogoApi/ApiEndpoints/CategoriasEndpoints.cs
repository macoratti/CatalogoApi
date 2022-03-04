using CatalogoApi.Context;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.ApiEndpoints;

public static class CategoriasEndpoints
{
    public static void MapCategoriasEndpoints(this WebApplication app)
    {
        app.MapPost("/categorias/", async (Categoria categoria, AppDbContext db) => {
            db.Categorias.Add(categoria);
            await db.SaveChangesAsync();

            return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
        }).WithTags("Categorias");

        app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync())
            .WithTags("Categorias")
            .RequireAuthorization();
        //.Include(p=> p.Produtos)
        //.ToListAsync());

        app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
        {
            return await db.Categorias.FindAsync(id)
                    is Categoria categoria
                        ? Results.Ok(categoria)
                        : Results.NotFound();
        })
          .WithTags("Categorias")
          .RequireAuthorization();

        app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
        {
            if (categoria.CategoriaId != id)
            {
                return Results.BadRequest();
            }

            var categoriaDB = await db.Categorias.FindAsync(id);

            if (categoriaDB is null) return Results.NotFound();

            //found, so update with incoming note n.
            categoriaDB.Nome = categoria.Nome;
            categoriaDB.Descricao = categoria.Descricao;

            await db.SaveChangesAsync();
            return Results.Ok(categoriaDB);
        }).WithTags("Categorias");


        app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) => {

            var categoria = await db.Categorias.FindAsync(id);

            if (categoria is not null)
            {
                db.Categorias.Remove(categoria);
                await db.SaveChangesAsync();
            }

            return Results.NoContent();

        }).WithTags("Categorias");
    }
}

