using AutoMapper;
using BoltCargo.Business.Services.Abstracts;
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
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;
        private readonly IMapper _mapper;

        public FeedBackController(IFeedBackService feedBackService, IMapper mapper)
        {
            _feedBackService = feedBackService;
            _mapper = mapper;
        }

        // GET: api/<FeedBackController>
        [HttpGet]
        public async Task<List<FeedBackDto>> Get()
        {
            var feedBacks = await _feedBackService.GetAllAsync();
            var feedBackDtos = _mapper.Map<List<FeedBackDto>>(feedBacks);

            return feedBackDtos;
        }

        // GET api/<FeedBackController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var feedBack = await _feedBackService.GetByIdAsync(id);

            if (feedBack == null)
            {
                return NotFound(new { message = "No feedback found with this id" });
            }

            var feedBackDto = _mapper.Map<FeedBackDto>(feedBack);
            return Ok(new { feedBack = feedBackDto });

        }

        [HttpGet("myFeedBacks/{userId}")]
        public async Task<IActionResult> GetMyFeedBacks(string userId)
        {
            var feedBacks = await _feedBackService.GetAllAsync();
            var myFeedBacks = feedBacks.Where(f => f.UserId == userId);

            if (myFeedBacks != null)
            {
                var myFeedBacksDto = _mapper.Map<List<FeedBackDto>>(myFeedBacks);
                return Ok(new { MyFeedBacks = myFeedBacksDto });
            }

            return NotFound(new { Message = "No feedbacks found with this id" });

        }

        [HttpGet("anotherFeedBacks/{userId}")]
        public async Task<IActionResult> GetAnotherFeedBacks(string userId) 
        {
            var feedBacks = await _feedBackService.GetAllAsync();
            var anotherFeedBacks = feedBacks.Where(f => f.UserId != userId).ToList();
            var anotherFeedBackDtos = _mapper.Map<List<FeedBackDto>>(anotherFeedBacks);
            return Ok(new { AnotherFeedBacks = anotherFeedBackDtos });
        }

        // POST api/<FeedBackController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FeedBackExtensionDto dto)
        {
            //var feedBack = new FeedBack
            //{
            //    Name = dto.Name,
            //    Content = dto.Content,
            //};
            var feedBack = _mapper.Map<FeedBack>(dto);
            await _feedBackService.AddAsync(feedBack);
            return Ok(new { message = "Feedback Added Successfully" });
        }

        // PUT api/<FeedBackController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FeedBackExtensionDto dto)
        {
            var feedBack = await _feedBackService.GetByIdAsync(id);

            if (feedBack != null)
            {
                feedBack.Name = dto.Name;
                feedBack.Content = dto.Content;

                await _feedBackService.UpdateAsync(feedBack);
                return Ok(new { message = "FeedBack Updated Successfully" });
            }

            return NotFound((new { message = "No feedback found with this id" }));
        }

        // DELETE api/<FeedBackController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var feedBack = await _feedBackService.GetByIdAsync(id);
            if (feedBack != null)
            {
                await _feedBackService.DeleteAsync(feedBack);
                return Ok(new { message = "FeedBack Deleted Successfully" });
            }
            return NotFound((new { message = "No feedback found with this id" }));
        }
    }
}
