   public class NewsletterService : INewsletterService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;

        public NewsletterService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService;
        }

        public Paged<Newsletter> PagedAll(int pageIndex, int pageSize)
        {
            Paged<Newsletter> pagedList = null;
            List<Newsletter> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Newsletters_PagedAll]",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Newsletter newsletter = MapSingleNewsletter(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (list == null)
                    {
                        list = new List<Newsletter>();
                    }
                    list.Add(newsletter);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Newsletter>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Newsletter> QueryPaged(int pageIndex, int pageSize, string query)
        {
            Paged<Newsletter> pagedList = null;
            List<Newsletter> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Newsletters_Query_Paged]",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@Query", query);
                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Newsletter newsletter = MapSingleNewsletter(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (list == null)
                    {
                        list = new List<Newsletter>();
                    }
                    list.Add(newsletter);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Newsletter>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public void Delete(int Id)
        {
            string procName = "[dbo].[Newsletters_Delete_ById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", Id);
            }, returnParameters: null);
        }

        public void DeleteComposite(int Id)
        {
            string procName = "[dbo].[Newsletters_Delete_ById_Composite]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", Id);
            }, returnParameters: null);
        }

        public int Insert(NewsletterAddRequest model)
        {
            int Id = 0;

            string procName = "[dbo].[Newsletters_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;
                int.TryParse(oId.ToString(), out Id);

                Console.WriteLine("");
            });
            return Id;
        }

        public int InsertComposite(NewsletterAddRequest model)
        {
            int Id = 0;
            DataTable contentTable = MapContentToTable(model);

            string procName = "[dbo].[Newsletters_Insert_Composite]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@BatchTable", contentTable);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int); 
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;
                int.TryParse(oId.ToString(), out Id);

                Console.WriteLine("");
            });

            return Id;
        }

        private DataTable MapContentToTable(NewsletterAddRequest contentToMap)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TemplateKeyId", typeof(string));
            dt.Columns.Add("ContentOrder", typeof(string)); 
            dt.Columns.Add("Value", typeof(string));

            if (contentToMap != null)
            {
                foreach (NewsletterContent content in contentToMap.Content)
                {
                    DataRow dr = dt.NewRow();
                    int startingIndex = 0;

                    dr.SetField(startingIndex++, content.NewsletterTemplateKey.Id);
                    dr.SetField(startingIndex++, content.ContentOrder);
                    dr.SetField(startingIndex++, content.Value);
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        private DataTable MapUpdateContentToTable(NewsletterAddRequest contentToMap)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("TemplateKeyId", typeof(string));
            dt.Columns.Add("ContentOrder", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            if (contentToMap != null)
            {
                foreach (NewsletterContent content in contentToMap.Content)
                {
                    DataRow dr = dt.NewRow();
                    int startingIndex = 0;

                    dr.SetField(startingIndex++, content.Id);
                    dr.SetField(startingIndex++, content.NewsletterTemplateKey.Id);
                    dr.SetField(startingIndex++, content.ContentOrder);
                    dr.SetField(startingIndex++, content.Value);
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public void Update(NewsletterUpdateRequest model)
        {
            string procName = "[dbo].[Newsletters_Update]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", model.Id);
                    AddCommonParams(model, col);
                },
            returnParameters: null);
        }

        public void UpdateComposite(NewsletterUpdateRequest model)
        {
            DataTable contentTable = MapUpdateContentToTable(model);

            string procName = "[dbo].[Newsletters_Update_Composite]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", model.Id);
                    AddCommonParams(model, col);
                    col.AddWithValue("@BatchTable", contentTable);
                },
            returnParameters: null);
        }

        private static void AddCommonParams(NewsletterAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@TemplateId", model.TemplateId);
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@CoverPhoto", model.CoverPhoto);
            col.AddWithValue("@DateToPublish", model.DateToPublish);
            col.AddWithValue("@DateToExpire", model.DateToExpire);
            col.AddWithValue("@CreatedBy", model.CreatedBy);
        }

        private Newsletter MapSingleNewsletter(IDataReader reader, ref int startingIndex)
        {
            Newsletter aNewsletter = new Newsletter();
            aNewsletter.User = new User();
            aNewsletter.NewsletterTemplate = new NewsletterTemplate();

            aNewsletter.Id = reader.GetSafeInt32(startingIndex++);
            aNewsletter.NewsletterTemplate.Id = reader.GetSafeInt32(startingIndex++);
            aNewsletter.Name = reader.GetSafeString(startingIndex++);
            aNewsletter.CoverPhoto = reader.GetSafeString(startingIndex++);
            aNewsletter.DateToPublish = reader.GetSafeDateTime(startingIndex++);
            aNewsletter.DateToExpire = reader.GetSafeDateTime(startingIndex++);
            aNewsletter.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aNewsletter.DateModified = reader.GetSafeDateTime(startingIndex++);
            aNewsletter.User.UserId  = reader.GetSafeInt32(startingIndex++);

            return aNewsletter;
        }
    }