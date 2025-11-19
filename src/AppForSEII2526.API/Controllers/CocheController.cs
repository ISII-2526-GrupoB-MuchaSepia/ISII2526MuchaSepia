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

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CocheParaAlquilerDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaAlquilar(string? modelo,double? precioAlquiler)
        {
            var coches= await _context.Coches
                                .Include(c => c.Modelo)
                .Where(c=>
                    (modelo == null || c.Modelo.Name.Contains(modelo)) &&
                    (precioAlquiler == null || c.PrecioAlquiler <= precioAlquiler))
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
     
    
    
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CocheParaReseñarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaReseñar(string? claseCoche, string? color)
        {
            var query = _context.Coches
                .Include(c => c.Modelo)
                .Where(c =>
                    (claseCoche == null || c.ClaseCoche.Contains(claseCoche)) &&
                    (color == null || c.Color.Contains(color))
                )
                .OrderBy(c => c.ClaseCoche);

            var coches = await query
                .Select(c => new CocheParaReseñarDTO(
                    c.Id,
                    c.ClaseCoche,
                    c.Color,
                    c.Descripcion,
                    c.Modelo != null ? c.Modelo.Name : ""
                ))
                .ToListAsync();

            return Ok(coches);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CocheParaCompraDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaComprar( string? modeloCoche, string? fcolor)
        {
            IList<CocheParaCompraDTO> selectCoche = await _context.Coches

                .Include(m => m.Modelo)
                .Include(m => m.ComprarItems).ThenInclude(pi => pi.Comprar)

                .Where(m => m.CantidadCompra > 0 && (modeloCoche == null || m.Modelo.Name.Equals(modeloCoche))) 
                .OrderBy(m => m.Modelo)

                .Select(m => new CocheParaCompraDTO(m.Id, m.Modelo.Name, m.PrecioCompra, m.Color,m.Fabricante,m.TipoCombustible ))
                .ToListAsync();

            return Ok(selectCoche);
        }
    }
}
