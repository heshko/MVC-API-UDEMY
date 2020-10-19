﻿using ParkyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class NationalParkRepository :Repository.IRepository.Repository<NationalPark>,INationaParkRepository
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public NationalParkRepository(IHttpClientFactory httpClientFactory) :base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
