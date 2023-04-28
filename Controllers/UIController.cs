using AutoMapper;
using GitRepositoryTracker.DTO;
using GitRepositoryTracker.Interfaces;
using GitRepositoryTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitRepositoryTracker.Controllers
{
    /// <summary>
    /// UIController handles API endpoints for retrieving repository, topic, and language information.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UIController : ControllerBase
    {
        private readonly IUIGenericRepository _uIGenericRepository;
        private readonly IMapper _mapper;

        public UIController(IUIGenericRepository uIGenericRepository, IMapper mapper)
        {
            _uIGenericRepository = uIGenericRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of all repositories.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A response indicating whether the operation was successful and a paginated list of RepositoryDto objects.</returns>
        [Authorize]
        [HttpGet("all-repositories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<RepositoryDto>>> GetAllRepostories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var repositories = await _uIGenericRepository.GetAllRepositoriesAsync(pageNumber, pageSize);
            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<RepositoryDto>(repositories));
        }

        /// <summary>
        /// Retrieves a paginated list of repositories sorted by the updated_at field.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A response indicating whether the operation was successful and a paginated list of RepositoryDto objects sorted by updated_at.</returns>
        [Authorize]
        [HttpGet("by-updated-at")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<RepositoryDto>>> GetAllByUpdatedAt([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var repositories = await _uIGenericRepository.GetAllByDateAsync(pageNumber, pageSize);
            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<RepositoryDto>(repositories));
        }

        /// <summary>
        /// Retrieves a paginated list of repositories that have a specified topic.
        /// </summary>
        /// <param name="topicName">The name of the topic to filter by.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A response indicating whether the operation was successful and a paginated list of RepositoryDto objects with the specified topic.</returns>
        [Authorize]
        [HttpGet("topic/{topicName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<RepositoryDto>>> GetAllByTopic(string topicName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrEmpty(topicName))
            {
                return BadRequest("TopicDto parameter is required");
            }

            var repositories = await _uIGenericRepository.GetAllByTopicAsync(topicName, pageNumber, pageSize);

            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }
            return Ok(new PaginatedResponse<RepositoryDto>(repositories));
        }

        /// <summary>
        /// Retrieves a paginated list of repositories sorted by the number of forks.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A response indicating whether the operation was successful and a paginated list of RepositoryDto objects sorted by the number of forks.</returns>
        [Authorize]
        [HttpGet("by-forks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<RepositoryDto>>> GetAllByNumberOfForks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var repositories = await _uIGenericRepository.GetAllByForksAsync(pageNumber, pageSize);
            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<RepositoryDto>(repositories));

        }

        /// <summary>
        /// Retrieves a paginated list of repositories sorted by the number of stars.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A response indicating whether the operation was successful and a paginated list of RepositoryDto objects sorted by the number of stars.</returns>
        [Authorize]
        [HttpGet("by-stars")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<RepositoryDto>>> GetAllByNumberOfStars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var repositories = await _uIGenericRepository.GetAllByStarsAsync(pageNumber, pageSize);
            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<RepositoryDto>(repositories));

        }

        /// <summary>
        /// Retrieves a paginated list of repositories that use a specified programming language.
        /// </summary>
        /// <param name="languageName">The name of the programming language to filter by.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A response indicating whether the operation was successful and a paginated list of RepositoryDto objects using the specified programming language.</returns>
        [Authorize]
        [HttpGet("language/{languageName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<RepositoryDto>>> GetAllByLanguage(string languageName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrEmpty(languageName))
            {
                return BadRequest("Language parameter is required");
            }
            var repositories = await _uIGenericRepository.GetAllByLanguageAsync(languageName, pageNumber, pageSize);
            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<RepositoryDto>(repositories));

        }

        /// <summary>
        /// Retrieves a paginated list of all topics.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A response indicating whether the operation was successful and a paginated list of TopicDto objects.</returns>
        [Authorize]
        [HttpGet("all-topics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<TopicDto>>> GetAllTopics([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var topics = await _uIGenericRepository.GetAllTopicsAsync(pageNumber, pageSize);
            if (topics == null || !topics.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<TopicDto>(topics));
        }

        /// <summary>
        /// Retrieves a paginated list of all Languages.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A response indicating whether the operation was successful and a paginated list of LanguageDto objects.</returns>
        [Authorize]
        [HttpGet("all-languages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<LanguageDto>>> GetAllLanguages([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var languages = await _uIGenericRepository.GetAllLanguagesAsync(pageNumber, pageSize);
            if (languages == null || !languages.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<LanguageDto>(languages));
        }
    }
}
