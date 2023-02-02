    [Route("api/newsletters")]
    [ApiController]
    public class NewsletterApiController : BaseApiController
    {
        private INewsletterService _service = null;
        private IAuthenticationService<int> _authService = null;
        public NewsletterApiController(INewsletterService service
            , ILogger<NewsletterApiController> logger
            , IAuthenticationService<int> authService) : base(logger)

        {
            _service = service;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<ItemsResponse<Paged<Newsletter>>> PagedAll(int pageIndex, int pageSize)
        {
            int code = 200;
            ActionResult result = null;
            try
            {
                Paged<Newsletter> paged = _service.PagedAll(pageIndex, pageSize);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));

                }
                else
                {
                    ItemResponse<Paged<Newsletter>> response = new ItemResponse<Paged<Newsletter>>();
                    response.Item = paged;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }

            return StatusCode(code, result);
        }

        [AllowAnonymous]
        [HttpGet("query")]
        public ActionResult<ItemsResponse<Paged<Newsletter>>> QueryPaged(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            ActionResult result = null;
            try
            {
                Paged<Newsletter> paged = _service.QueryPaged(pageIndex, pageSize, query);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));

                }
                else
                {
                    ItemResponse<Paged<Newsletter>> response = new ItemResponse<Paged<Newsletter>>();
                    response.Item = paged;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }

            return StatusCode(code, result);
        }

        [HttpDelete("delete/{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {

                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpDelete("composite/{id:int}")]
        public ActionResult<SuccessResponse> DeleteComposite(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteComposite(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {

                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Insert(NewsletterAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int id = _service.Insert(model);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPost("composite")]
        public ActionResult<ItemResponse<int>> InsertComposite(NewsletterAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int id = _service.InsertComposite(model);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(NewsletterUpdateRequest model)
        {

            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }

        [HttpPut("composite/{id:int}")]
        public ActionResult<ItemResponse<int>> UpdateComposite(NewsletterUpdateRequest model)
        {

            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.UpdateComposite(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }
    }