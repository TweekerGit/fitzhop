using System;
using FittShop.Domain;
using FittShop.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FittShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly ItemsDto _itemsDto;
        private readonly ItemDto _itemDto;

        public HomeController(DataManager dataManager, ItemsDto itemsDto, ItemDto itemDto)
        {
            _dataManager = dataManager;
            _itemsDto = itemsDto;
            _itemDto = itemDto;
        }

        [HttpGet]
        public IActionResult Index(int category)
        {
            
            return View(_itemsDto);
        }

        [HttpGet]
        [Route("good")]
        public IActionResult Good(Guid id)
        {
            var item = _dataManager.ServiceItems.GetServiceItemById(id);
            _itemDto.Item = item;
            return View(_itemDto);
        }

        [HttpGet]
        [Route("contacts")]
        public IActionResult Contacts()
        {
            return View(_itemsDto);
        }
    }
}