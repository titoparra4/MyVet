using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVet.Common.Models;
using MyVet.Web.Data;
using MyVet.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Controllers.API
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AgendaController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;

        public AgendaController(
            DataContext dataContext,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
        }

        [HttpPost]
        [Route("GetAgendaForOwner")]
        public async Task<IActionResult> GetAgendaForOwner(EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agendas = await _dataContext.Agendas
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .ThenInclude(p => p.PetType)
                .Where(a => a.Date >= DateTime.Today.ToUniversalTime())
                .OrderBy(a => a.Date)
                .ToListAsync();

            var response = new List<AgendaResponse>();
            foreach (var agenda in agendas)
            {
                var agendaRespose = new AgendaResponse
                {
                    Date = agenda.Date,
                    Id = agenda.Id,
                    IsAvailable = agenda.IsAvailable
                };

                if (agenda.Owner != null)
                {
                    if (agenda.Owner.User.Email.ToLower().Equals(emailRequest.Email.ToLower()))
                    {
                        agendaRespose.Owner = _converterHelper.ToOwnerResposne(agenda.Owner);
                        agendaRespose.Pet = _converterHelper.ToPetResponse(agenda.Pet);
                        agendaRespose.Remarks = agenda.Remarks;
                    }
                    else
                    {
                        agendaRespose.Owner = new OwnerResponse { FirstName = "Reserved" };
                    }
                }

                response.Add(agendaRespose);
            }

            return Ok(response);
        }
    }
}
