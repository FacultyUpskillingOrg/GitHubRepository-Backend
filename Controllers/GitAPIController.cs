using AutoMapper;
using GitRepositoryTracker.DTO;
using GitRepositoryTracker.Interfaces;
using GitRepositoryTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitRepositoryTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitAPIController : ControllerBase
    {
        private readonly IGitAPIRepository _gitAPIRepository;
        private readonly IMapper _mapper;
        private readonly IGitHubAPIService _gitHubAPIService;
        private readonly IRepositoryDeserializer _repositoryDeserializer;

        public GitAPIController(IGitAPIRepository gitAPIRepository, IMapper mapper, IGitHubAPIService gitHubAPIClient, IRepositoryDeserializer repositoryDeserializer)
        {
            _gitAPIRepository = gitAPIRepository;
            _mapper = mapper;
            _gitHubAPIService = gitHubAPIClient;
            _repositoryDeserializer = repositoryDeserializer;
        }

        [Authorize]
        [HttpPost("add_topics")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddTopics(IEnumerable<TopicDto> topicDtos)
        {
            if (topicDtos == null || !topicDtos.Any())
            {
                return BadRequest("No topics provided");
            }

            var topics = _mapper.Map<IEnumerable<Topic>>(topicDtos);

            await _gitAPIRepository.Addtopics(topics);

            return Ok();
        }

        [Authorize]
        [HttpGet("repository/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RepositoryDto>> GetRepositoryById(string id)
        {
            var repository = await _gitAPIRepository.GetRepositoryById(id);

            if (repository == null)
            {
                return NotFound();
            }
            var repositoryDto = _mapper.Map<RepositoryDto>(repository);

            return Ok(repositoryDto);
        }

        [Authorize]
        [HttpGet("gettopic/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TopicDto>> GetTopicById(int id)
        {
            var topic = await _gitAPIRepository.GetTopicById(id);

            if (topic == null)
            {
                return NotFound();
            }
            var topicDto = _mapper.Map<TopicDto>(topic);

            return Ok(topicDto);
        }

        [Authorize]
        [HttpPost("repositories")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]      
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRepositories(int size, int page, int perPage)
        {

            try
            {
                var repositories = await _gitHubAPIService.GetAllRepositoriesBySize(size, page, perPage);
                await _gitAPIRepository.AddRepositories(repositories);

                return Ok("Repositories added to database successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding repositories to database: {ex.Message}");
            }


        }
    }
}
