using AutoMapper;
using GitRepositoryTracker.DTO;
using GitRepositoryTracker.Interfaces;
using GitRepositoryTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitRepositoryTracker.Controllers
{
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

        [Authorize]
        [HttpGet("all-repositories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<RepositoryDto>>> GetAllRepostories([FromQuery]int pageNumber=1, [FromQuery]int pageSize=10)
        {

            var repositories = await _uIGenericRepository.GetAllRepositoriesAsync(pageNumber,pageSize);
            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<RepositoryDto>(repositories));
        }

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

            var repositories = await _uIGenericRepository.GetAllByTopicAsync(topicName,pageNumber,pageSize);

            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }
            return Ok(new PaginatedResponse<RepositoryDto>(repositories));
        }

        [Authorize]
        [HttpGet("by-forks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<RepositoryDto>>> GetAllByNumberOfForks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var repositories = await _uIGenericRepository.GetAllByForksAsync(pageNumber,pageSize);
            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<RepositoryDto>(repositories));

        }

        [Authorize]
        [HttpGet("by-stars")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<RepositoryDto>>> GetAllByNumberOfStars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var repositories = await _uIGenericRepository.GetAllByStarsAsync(pageNumber,pageSize);
            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<RepositoryDto>(repositories));

        }

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
            var repositories = await _uIGenericRepository.GetAllByLanguageAsync(languageName,pageNumber,pageSize);
            if (repositories == null || !repositories.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<RepositoryDto>(repositories));

        }

        [Authorize]
        [HttpGet("all-topics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResponse<TopicDto>>> GetAllTopics([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var topics = await _uIGenericRepository.GetAllTopicsAsync(pageNumber,pageSize);
            if (topics == null || !topics.Any())
            {
                return NotFound();
            }

            return Ok(new PaginatedResponse<TopicDto>(topics));
        }

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
