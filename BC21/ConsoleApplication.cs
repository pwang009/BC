using System;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BC21
{
    public class ConsoleApplication
    {
        private readonly IConfiguration _config;
        private readonly IBitCoinValidationService _service;
        private readonly Block _options;
        private readonly IMapper _mapper;

        public ConsoleApplication(
            IConfiguration configuration, 
            IBitCoinValidationService service,
            IOptions<Block> options,
            IMapper mapper)
        {
            _config = configuration;
            _service = service;
            _options = options.Value;
            _mapper = mapper;
        }

        public void Run()
        {
            var block = _mapper.Map<Block>(_options);
            block.ValidateBlockHash();
            //_service.ValidateBlockHash();
        }
    }
}
