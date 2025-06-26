global using System.Text;
global using System.Security.Cryptography;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.IdentityModel.Tokens;
global using AutoMapper;
global using FluentValidation;
global using MediatR;
global using Core.Application.Common.Extensions;
global using Core.Application.Common.Interfaces;
global using Core.Application.Interfaces.Dbo.ServiceInterfaces;

global using Core.Application.Interfaces.Dbo.RepositoryInterfaces;

global using Core.Application.Services.Dbo;

global using Core.Domain.Dbo;

global using Shared.DTOs.Dbo;

global using Shared.DTOs.Common;
global using Shared.DTOs.Common.Pagination;