// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using AutoMapper;
using CompanyEmployees.IDP.Entities;
using CompanyEmployees.IDP.Entities.ViewModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using EmailService;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CompanyEmployees.IDP.Pages.Create;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly TestUserStore _users;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IEmailSender _emailSender;

    [BindProperty]
    public InputModel Input { get; set; } = default!;



    public Index(
        IIdentityServerInteractionService interaction, IMapper mapper, SignInManager<User> signInManager, UserManager<User> userManager, IEmailSender emailSender)
       
    {
        // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
        
            
        _interaction = interaction;
        _mapper = mapper;
        _signInManager = signInManager;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public IActionResult OnGet(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return Page();
    }
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPost(string returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var user = _mapper.Map<User>(Input);
        
        var result = await _userManager.CreateAsync(user, Input.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return Page();
        }

        await _userManager.AddToRoleAsync(user, "Visitor");

        await _userManager.AddClaimsAsync(user, new List<Claim>{
            new Claim(JwtClaimTypes.GivenName, user.FirstName),
            new Claim(JwtClaimTypes.FamilyName, user.LastName),
            new Claim(JwtClaimTypes.Role, "Visitor"),
            new Claim(JwtClaimTypes.Address, user.Address),
            new Claim("country", user.Country)
        });

        return Redirect(returnUrl);
    }
}
