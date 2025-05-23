﻿using TerminiDomain.Core;
using TerminiWeb.Infrastructure.PlayerService.Dtos;

namespace TerminiWeb.Infrastructure.PlayerService.Models
{
	public class GetPlayerListRequest : RequestBase
	{
		public string Name { get; set; } = string.Empty;
		public string Surname { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public int? PlayerRating { get; set; }
	}

	public class GetPlayerListResponse : ResponseBase<GetPlayerListRequest>
	{
		public IEnumerable<PlayerDto>? Players { get; set; }
	}
}
