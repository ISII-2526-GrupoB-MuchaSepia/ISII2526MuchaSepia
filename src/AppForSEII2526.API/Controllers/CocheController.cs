using AppForSEII2526.API.DTOs.CocheDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
﻿using AppForSEII2526.Shared.CocheDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppForSEII2526.API.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AppForSEII2526.API.Models;
using System.Drawing;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CocheController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CocheController> _logger;

        public CocheController(ApplicationDbContext context, ILogger<CocheController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet] //Get_Coches_Alquiler_test (GET)
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CocheParaAlquilerDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaAlquilar(string? modelo,double? precioAlquiler) //son opcionales(?) y se usan como filtros
        {
            var coches= await _context.Coches //accede a la tabla/DbSet de coches en la base de datos y  carga también la entidad relacionada Modelo para poder usar c.Modelo.Name
                                .Include(c => c.Modelo)
                .Where(c=>
                    (modelo == null || c.Modelo.Name.Contains(modelo)) && //Si modelo es null, no se filtra por modelo.Si modelo tiene valor, busca coches cuyo nombre de modelo contenga ese texto
                    (precioAlquiler == null || c.PrecioAlquiler <= precioAlquiler))//Si precioAlquiler es null, no se filtra por precio.Si tiene valor, devuelve solo coches con precio de alquiler menor o igual a ese valor.


                .Select(c => new CocheParaAlquilerDTO(
                c.Id,
                c.Modelo.Name ,
                    c.Color,
                    c.PrecioAlquiler,
                    c.TipoCombustible,
                    c.Fabricante))
                .ToListAsync();
            _logger.LogInformation("Coches para alquilar con modelo: {modelo} y precio de alquiler {precioAlquiler}");
            return Ok(coches);

        }



        // MÉTODO GET PARA OBTENER COCHES DISPONIBLES PARA RESEÑAR, CON FILTROS OPCIONALES POR CLASE Y COLOR
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CocheParaReseñarDTO>), (int)HttpStatusCode.OK)] // DEVUELVE LISTA DE DTO DE COCHES CON STATUS 200
        public async Task<ActionResult> GetCochesParaReseñar(string? claseCoche, string? color)
        {
            // CREA CONSULTA FILTRANDO LOS COCHES POR CLASE Y COLOR SI SE PROPORCIONAN
            var query = _context.Coches
                .Include(c => c.Modelo) // INCLUYE DATOS DEL MODELO DEL COCHE (RELACIÓN NAVEGACIONAL)
                .Where(c =>
                    (claseCoche == null || c.ClaseCoche.Contains(claseCoche)) && // FILTRA POR CLASE SI SE INDICA
                    (color == null || c.Color.Contains(color)) // FILTRA POR COLOR SI SE INDICA
                )
                .OrderBy(c => c.ClaseCoche); // ORDENA RESULTADOS POR CLASE DE COCHE PARA MEJOR LECTURA

            // PROYECTA RESULTADOS A DTO SIMPLIFICADO Y OBTIENE RESULTADO ASINCRÓNICAMENTE
            var coches = await query
                .Select(c => new CocheParaReseñarDTO(
                    c.Id,
                    c.ClaseCoche,
                    c.Color,
                    c.Descripcion,
                    c.Modelo != null ? c.Modelo.Name : "" // PROTEGE SI NO HAY MODELO ASOCIADO
                ))
                .ToListAsync();

            return Ok(coches); // DEVUELVE RESPUESTA 200 CON LISTA DE COCHES FILTRADOS
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CocheParaCompraDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaComprar( string? fcolor, string? modeloCoche)//son opcionales(?) y se usan como filtros
        {   
            IList<CocheParaCompraDTO> selectCoche = await _context.Coches// // Consulta a la tabla Coches del contexto

                .Include(m => m.Modelo)// Incluye la entidad relacionada Modelo (para acceder a Modelo.Name)
                .Include(m => m.ComprarItems).ThenInclude(pi => pi.Comprar)// Incluye la relación ComprarItems y luego la entidad Comprar vinculada

                .Where(m =>
                         m.CantidadCompra > 0 &&
                        (fcolor == null || m.Color.Contains(fcolor)) && //Si color , no se filtra por color .Si tiene valor, devuelve solo coches con color igual al dado
                        (modeloCoche == null || m.Modelo.Name.Contains(modeloCoche))//Si modelo es null, no se filtra por modelo.Si modelo tiene valor, busca coches cuyo nombre de modelo contenga ese texto
                 )
                .OrderBy(m => m.Modelo)

                .Select(m => new CocheParaCompraDTO(m.Id, m.Modelo.Name, m.PrecioCompra, m.Color,m.TipoCombustible, m.Fabricante,m.Descripcion))// Selecciona solo los campos necesarios para el DTO CocheParaCompraDTO
                .ToListAsync(); // Ejecuta la consulta en la base de datos de forma asíncrona

            return Ok(selectCoche);
        }

    }
}
