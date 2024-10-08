﻿using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings;

public static class CategoriaDTOMappingExtensions
{
    public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
    {
        if (categoria is null)
            return null;

        return new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl,
        };
    }

    public static Categoria? ToCategoria(this CategoriaDTO categoriaDTO)
    {
        if (categoriaDTO is null)
            return null;

        return new Categoria()
        {
            CategoriaId = categoriaDTO.CategoriaId,
            Nome = categoriaDTO.Nome,
            ImagemUrl = categoriaDTO.ImagemUrl,
        };
    }

    public static IEnumerable<CategoriaDTO>? ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
    {
        if (categorias is null || !categorias.Any())
            return null;

        var categoriasDTO = new List<CategoriaDTO>();

        foreach (var categoria in categorias)
        {
            CategoriaDTO categoriaDTO = new CategoriaDTO();
            categoriaDTO.CategoriaId = categoria.CategoriaId;
            categoriaDTO.Nome = categoria.Nome;
            categoriaDTO.ImagemUrl = categoria.ImagemUrl;

            categoriasDTO.Add(categoriaDTO);
        }

        return categoriasDTO;
    }
}