using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace mangaQuotes.Data.Services
{
    public class CharacterService
{
    #region Private members
    private QuoteDbContext dbContext;
    #endregion

    #region Constructor
    public CharacterService(QuoteDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Public methods
    /// <summary>
    /// This method returns the list of characters
    /// </summary>
    /// <returns></returns>
    public async Task<List<Character>> GetCharactersAsync()
    {
        return await dbContext.Characters.ToListAsync();
    }

    /// <summary>
    /// This method add a new character to the DbContext and saves it
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public async Task<Character> AddCharacterAsync(Character character)
    {
        try
        {
            dbContext.Characters.Add(character);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
        return character;
    }

    /// <summary>
    /// This method update and existing character and saves the changes
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public async Task<Character> UpdateCharacterAsync(Character character)
    {
        try
        {
            var characterExist = dbContext.Characters.FirstOrDefault(p => p.Id == character.Id);
            if (characterExist != null)
            {
                dbContext.Update(character);
                await dbContext.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            throw;
        }
        return character;
    }

    /// <summary>
    /// This method removes and existing character from the DbContext and saves it
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public async Task DeleteCharacterAsync(Character character)
    {
        try
        {
            dbContext.Characters.Remove(character);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}
}