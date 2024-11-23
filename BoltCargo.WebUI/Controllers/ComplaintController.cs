using AutoMapper;
using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Business.Services.Concretes;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoltCargo.WebUI.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IComplaintService _complaintService;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        public ComplaintController(IMapper mapper, IComplaintService complaintService, ICustomIdentityUserService customIdentityUserService )
        {
            _mapper = mapper;
            _complaintService = complaintService;
            _customIdentityUserService = customIdentityUserService;
        }

        [HttpGet]
        public async Task<List<ComplaintDto>> Get()
        {
            var complaints = await _complaintService.GetAllAsync();
            var complaintsDto = _mapper.Map<List<ComplaintDto>>(complaints);
            return complaintsDto;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var complaint = await _complaintService.GetByIdAsync(id);
            if (complaint != null)
            {
                var complaintDto = _mapper.Map<ComplaintDto>(complaint);
                return Ok(new { Complaint = complaintDto });
            }
            return NotFound(new { Message = "No complaint found with this id" });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ComplaintAddDto dto)
        { 
            var complaint = _mapper.Map<Complaint>(dto);
            await _complaintService.AddAsync(complaint);

            return Ok(new { Message = "Complaint Added Successfully." });
        }
    }
}
