using System;
using System.Collections.Generic;
using System.Text;
using TARge25shop.Core.Dto;

namespace TARge25shop.Core.ServiceInterface
{
    public interface IChuckNorrisServices
    {
        Task<ChuckNorrisRootDto> ChuckNorrisResultHttpClient();
    }
}