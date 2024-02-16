using AutoMapper;
using GitRepositoryTracker.DTO;
using GitRepositoryTracker.Interfaces;
using GitRepositoryTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitRepositoryTracker.Controllers
{
    /// <summary>
    /// GitAPIController handles API endpoints related to topics and repositories.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GitAPIController : ControllerBase
    {
        private readonly IGitAPIRepository _gitAPIRepository;
        private readonly IMapper _mapper;
        private readonly IGitHubAPIService _gitHubAPIService;

        public GitAPIController(IGitAPIRepository gitAPIRepository, IMapper mapper, IGitHubAPIService gitHubAPIClient)
        {
            _gitAPIRepository = gitAPIRepository;
            _mapper = mapper;
            _gitHubAPIService = gitHubAPIClient;

        }

        /// <summary>
        /// Adds Topics to the database.
        /// </summary>
        /// <param name="topicDtos"> A collection of topics to add.</param>
        /// <returns>A response indicating whether the operation was successful.</returns>
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

        /// <summary>
        ///Retrieves a repository by its ID. 
        /// </summary>
        /// <param name="id"> ID of the repository to be retrieved</param>
        /// <returns>A response containing the repository DTO or an error message.</returns>
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

        /// <summary>
        /// Retrieves a topic by its ID.
        /// </summary>
        /// <param name="id">ID of the topic to be retrieved</param>
        /// <returns>A response containing the topic DTO or an error message.</returns>
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

        /// <summary>
        /// Adds repositories to the database based on the provided parameters.
        /// </summary>
        /// <param name="size">The size in KBs of the repositories to retrieve.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="perPage">The number of repositories per page.</param>
        /// <returns>A response indicating whether the operation was successful.</returns>
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
