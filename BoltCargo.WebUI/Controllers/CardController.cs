﻿using AutoMapper;
using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Business.Services.Concretes;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.AutoMappers;
using BoltCargo.WebUI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoltCargo.WebUI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICardService _cardService;

        public CardController(IMapper mapper, ICardService cardService)
        {
            _mapper = mapper;
            _cardService = cardService;
        }

        [HttpGet]
        public async Task<List<CardDto>> Get()
        {
            var cards = await _cardService.GetAllAsync();
            var cardsDto = _mapper.Map<List<CardDto>>(cards);
            return cardsDto;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var card = await _cardService.GetByIdAsync(id);
            if (card != null)
            {
                var cardDto = _mapper.Map<CardDto>(card);
                return Ok(new { CardDto = cardDto });
            }
            return NotFound(new { Message = "No card found with this id" });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CardAddDto dto)
        {
            var card = _mapper.Map<Card>(dto);
            await _cardService.AddAsync(card);

            return Ok(new { Message = "Card Added Successfully." });
        }

        [Authorize(Roles = "Client")]
        [HttpPut("{cardNumber}")]
        public async Task<IActionResult> UpdateCard(string cardNumber, [FromBody] CardUpdateDto dto)
        {
            var card = await _cardService.GetByCardNumberAsync(cardNumber,dto.BankName);
            if (card != null)
            {
                card.Balance = dto.Balance;

                await _cardService.UpdateAsync(card);
                return Ok(new { Message = "Card Updated Successfully" });
            }

            return NotFound(new { message = "No card found with these card number and bank name" });
        }
    }
}
