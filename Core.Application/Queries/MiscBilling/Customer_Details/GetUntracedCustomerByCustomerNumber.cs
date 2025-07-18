﻿using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.Customer_Details
{
public class GetUntracedCustomerByCustomerNumber : IRequest<CustomerDetailsDTO>
{
    public string? customerNumber { get; set; }
    public string? dbCode { get; set; }
    public string? locCode { get; set; }

    public class Handler : IRequestHandler<GetUntracedCustomerByCustomerNumber, CustomerDetailsDTO>
    {
        private readonly ICustomerDetailsRepository _repository;

        public Handler(ICustomerDetailsRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomerDetailsDTO> Handle(GetUntracedCustomerByCustomerNumber request, CancellationToken cancellationToken)
        {

            var result = await _repository.GetUntracedCustomer(request.customerNumber, request.dbCode, request.locCode);
            return result;

        }
    }
}
}

