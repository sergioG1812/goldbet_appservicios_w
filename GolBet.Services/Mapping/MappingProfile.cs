using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GolBet.Entities;
using GolBet.Services.DTOs;

namespace GolBet.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Match, MatchDto>();
        CreateMap<Bet, BetDto>()
    .ForMember(dto => dto.MatchHomeTeamName, o => o.MapFrom(b => b.Match.HomeTeam.Name))
    .ForMember(dto => dto.MatchAwayTeamName, o => o.MapFrom(b => b.Match.AwayTeam.Name))
    .ForMember(dto => dto.MatchDate, o => o.MapFrom(b => b.Match.Date))
    .ForMember(dto => dto.PotentialPayout, o => o.MapFrom(b => b.Amount * b.OddsAtPlacement));

        CreateMap<Match, MatchDetailDto>()
              .ForMember(dto => dto.TotalBets,
                         options => options.MapFrom(match => match.Bets.Count));

        CreateMap<Team, TeamDto>();
        CreateMap<TeamFormDto, Team>().ReverseMap();
        CreateMap<MatchFormDto, Match>().ReverseMap();
    }
}