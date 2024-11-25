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
        public ComplaintController(IMapper mapper, IComplaintService complaintService, ICustomIdentityUserService customIdentityUserService)
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

            foreach (var complaint in complaintsDto)
            {
                var sender = await _customIdentityUserService.GetByIdAsync(complaint.SenderId);
                var receiver = await _customIdentityUserService.GetByIdAsync(complaint.ReceiverId);

                complaint.Sender = sender;
                complaint.Receiver = receiver;
            }

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

        [HttpGet("receiverComplaintsCount/{id}")]
        public async Task<IActionResult> GetReceiverComplaintsCount(string id)
        {
            var count = await _complaintService.GetReceiverComplaintsCountAsync(id);
            return Ok(new { Count = count });
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
