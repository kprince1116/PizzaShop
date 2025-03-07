using System.IdentityModel.Tokens.Jwt;
using BAL.Interfaces;
using BCrypt.Net;
using DAL.Interfaces;
using DAL.Models;

// using Pizzashop.DAL.Models;

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Pizzashop.DAL.ViewModels;

namespace BAL.Services;


public class UserDetails : IUserDetails
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserDetails(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<ProfileViewmodel> GetUserProfile(string email)
    {

        var existingUser = await _userRepository.GetUserByEmailForProfile(email);

        if (existingUser == null)
        {
            return null;
        }

        return new ProfileViewmodel
        {
            Firstname = existingUser.Firstname,
            Lastname = existingUser.Lastname,
            Email = existingUser.Email,
            Phonenumber = existingUser.Phonenumber,
            Address = existingUser.Address,
            City = existingUser.City,
            Zipcode = existingUser.Zipcode,
            userrole = existingUser.UserroleNavigation?.Rolename
        };

    }

    public async Task<bool> UpdateProfile(ProfileViewmodel model)
    {
        var existingUser = await _userRepository.GetUserByEmailForProfile(model.Email);

        if (existingUser == null)
        {
            return false;
        }

        existingUser.Firstname = model.Firstname;
        existingUser.Lastname = model.Lastname;
        existingUser.Phonenumber = model.Phonenumber;
        existingUser.Address = model.Address;
        existingUser.City = model.City;
        existingUser.Zipcode = model.Zipcode;
        // existingUser.Userrole = model.userrole;
        existingUser.Email = model.Email;

        await _userRepository.updateUser(existingUser);

        return true;
    }

    public List<Country> GetCountries()
    {
        return _userRepository.GetCountries();
    }

    public List<State> GetStates(int countryId)
    {
        return _userRepository.GetStates(countryId);
    }

    public List<City> GetCities(int stateId)
    {
        return _userRepository.GetCities(stateId);
    }

    public async Task<ChangePasswordviewmodel> ChangePassword(string email, ChangePasswordviewmodel model)
    {
        var existingUser = await _userRepository.GetUserByEmailForChangePassword(email);

        if (existingUser == null)
        {
            return model;
        }

        if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, existingUser.Password))
        {
            throw new Exception("Current Password is wrong");
        }
        else
        {
            existingUser.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await _userRepository.UpdateUserforChangePassword(existingUser);
        }


        return model;
    }


}
