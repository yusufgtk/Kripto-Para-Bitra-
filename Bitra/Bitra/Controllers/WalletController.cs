
using AutoMapper;
using Business.Abstracts;
using Entitiy.Entites;
using Entity.Dtos;
using Microsoft.AspNetCore.Mvc;
using Repository.Responses;

namespace Bitra.Controllers
{
    [ApiController]
    [Route("bitra/wallet")]
    public class WalletController : ControllerBase
    {
        private readonly IBlockchainService _blockcahinService;
        private readonly IMapper _mapper;
        public WalletController(IBlockchainService blockcahinService, IMapper mapper)
        {
            _blockcahinService = blockcahinService;
            _mapper = mapper;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateWallet([FromBody] GenerateWalletDto generateWalletDto)
        {
            Wallet? newWallet = await _blockcahinService.CreateWallet(generateWalletDto.PrivateKey);
            if (newWallet is null)
                return NotFound("Wallet is not created!");
            WalletDto newWalletDto = _mapper.Map<WalletDto>(newWallet);
            return Ok(newWalletDto);
        }

        [HttpGet("{address}")]
        public async Task<IActionResult> GetUserWallet([FromRoute(Name = "address")] string address)
        {
            Wallet? myWallet = await _blockcahinService.GetUserWalletByAddress(address);
            if (myWallet is null)
                return NotFound("Wallet is not found!");
            WalletDto myWalletDto = _mapper.Map<WalletDto>(myWallet);
            return Ok(myWalletDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> WalletLogin([FromBody] WalletLoginDto walletLoginDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            DataResponse<Wallet?> dataResponse = await _blockcahinService.GetWalletByWalletAddressAndPrivateKey(walletLoginDto.Address, walletLoginDto.PrivateKey);
            //WalletDto myWalletDto = _mapper.Map<WalletDto>(myWallet);
            return StatusCode(dataResponse.StatusCode, dataResponse);
        }
    }

}