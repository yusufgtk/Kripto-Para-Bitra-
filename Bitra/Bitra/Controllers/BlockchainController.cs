using AutoMapper;
using Bitra.Hubs;
using Business.Abstracts;
using Entitiy.Entites;
using Entity.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Repository.Responses;

namespace Bitra.Controllers
{
    [ApiController]
    [Route("bitra/blockchain")]
    public class BlockchainController : ControllerBase
    {
        private readonly IBlockchainService _blockchainService;

        private readonly IHubContext<BlockchainHub> _hubContext;
        private readonly IMapper _mapper;
        public BlockchainController(IBlockchainService blockchainService, IHubContext<BlockchainHub> hubContext, IMapper mapper)
        {
            _blockchainService = blockchainService;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlockChain()
        {
            IEnumerable<Block> blocks = await _blockchainService.GetChainAsync();
            IEnumerable<BlockDto> blockDtos = _mapper.Map<IEnumerable<BlockDto>>(blocks);
            return Ok(blockDtos);
        }

        [HttpGet("pending-transaction")]
        public IActionResult GetPendingTransactions()
        {
            List<Transaction> pendingTransactions = _blockchainService.GetPendingTransactions();
            List<TransactionDto> pendingTransactionDtos = _mapper.Map<List<TransactionDto>>(pendingTransactions);
            return Ok(pendingTransactionDtos);
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateBlockChain()
        {
            bool isValid = await _blockchainService.IsChainValidAsync();
            await _hubContext.Clients.All.SendAsync("IsValidBlockChain", isValid);
            return Ok(isValid);
        }

        [HttpPost("transaction")]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionDto addTransactionDto)
        {
            ApiResponse response = await _blockchainService.ExecuteTransaction(addTransactionDto.Sender, addTransactionDto.Receiver, addTransactionDto.Amount, addTransactionDto.SignKey);
            if(response.StatusCode != 200) return StatusCode(response.StatusCode, response);
            Transaction transaction = new Transaction
            {
                Sender = addTransactionDto.Sender,
                Receiver = addTransactionDto.Receiver,
                Amount = addTransactionDto.Amount,
                Timestamp = DateTime.UtcNow,
                SignKey = addTransactionDto.SignKey,
            };

            await _blockchainService.CreatedTransactionAsync(transaction);

            TransactionDto transactionDto = _mapper.Map<TransactionDto>(transaction);

            await _hubContext.Clients.All.SendAsync("NewTransactionAdded", transactionDto);
            await _hubContext.Clients.All.SendAsync("PendingTransactions", _blockchainService.GetPendingTransactions());
            return Ok(new DataResponse<TransactionDto>(200,"Successed","İşlem başarılı.", transactionDto));
        }

    }
}
