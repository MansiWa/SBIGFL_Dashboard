USE [sbigeneral]
GO

/****** Object:  StoredProcedure [dbo].[proc_BorroAndRFR]    Script Date: 11-06-2024 12:06:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[proc_AssetsLiabilities]
(
@Id uniqueidentifier = NULL ,
@UserId uniqueidentifier = NULL ,
@al_id uniqueidentifier =NULL ,
@al_date datetime =null,
@al_fileid varchar(36)=null,
@al_docid varchar(36)=null,
@al_filename varchar(200)=null,
@al_count varchar(100)=null,
@al_updatedby varchar(36)=null,
@type varchar(100)=null,
@batchid varchar(100)=null,
@al_updateddate datetime = null,

@OperationType varchar(100) = NULL,
@OutcomeId int  = NULL  OUT,
@OutcomeDetail varchar(4000) = null out

)
as
begin
	If @OperationType='GetAll'
	begin
	BEGIN TRY
		BEGIN TRAN GetAllFileUpload 
		SET @OutcomeId = 1 SET @OutcomeDetail ='FileUpload'
		if @al_date is not null
		    select al_id,d.d_name as al_type,batchid,d.d_tablename,d.d_id,f.al_filename,al_date,f.al_count,f.al_fileid from  tbl_documentmaster d  left join tbl_ALUpload f on d.d_id=f.al_docid 
			where CONVERT(date, f.al_date) = CONVERT(date, @al_date)  and d_id in('F606ADF7-3362-43F9-BFE9-10E1C3E485F9')
			ORDER BY F.al_date DESC
		else
			select al_id,d.d_name as al_type,batchid,d.d_tablename,d.d_id,f.al_filename,al_date,f.al_count,f.al_fileid from  tbl_documentmaster d  left join tbl_ALUpload f on d.d_id=f.al_docid 
			where  d_id in('F606ADF7-3362-43F9-BFE9-10E1C3E485F9') and al_date is not null
			ORDER BY F.al_date DESC
			--union all
			--select d.d_name as al_doctype,d.d_tablename,d.d_id,f.al_filename,FORMAT(al_date, 'yyyy/MM/dd') AS al_date,f.al_count,f.al_fileid from  tbl_documentmaster d  left join tbl_BorroAndRFR f on d.d_id=f.al_docid where  d_id in('A0341EBE-2FB2-40D4-95D5-795DE789006B','539593FF-0807-479E-914B-D68497D5BC3A')

		COMMIT TRAN GetAllFileUpload
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN GetAllFileUpload

        SET @OutcomeId = ERROR_NUMBER()  SET @OutcomeDetail = ERROR_MESSAGE()
		
    END CATCH
	 SELECT @OutcomeId AS OutcomeId, @OutcomeDetail AS OutcomeDetail;
	 end

	 If @OperationType='Insert'
	begin
	BEGIN TRY
		BEGIN TRAN InsertFileUpload 
		SET @OutcomeId = 1 SET @OutcomeDetail ='File uploaded successfully'
			insert into tbl_ALUpload (al_id,al_fileid,al_docid,batchid,al_date,al_filename,al_count,al_createdby,al_updateddate,al_createddate,al_isactive)
			values(newid(),@al_fileid,@al_docid,@batchid,@al_date,@al_filename,@al_count,@UserId,@al_updateddate,@al_updateddate,'1')

		COMMIT TRAN InsertFileUpload
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN InsertFileUpload

        SET @OutcomeId = ERROR_NUMBER()  SET @OutcomeDetail = ERROR_MESSAGE()
		
    END CATCH
	 SELECT  @OutcomeDetail AS OutcomeDetail
	 SELECT @OutcomeId AS OutcomeId, @OutcomeDetail AS OutcomeDetail;
	 end


if @operationType='GetAssets'
begin
  begin TRY
begin TRAN GetAssets
SET @OutcomeId = 1 SET @OutcomeDetail ='Assets!'
		if(@al_date is null)
		begin
		select top 1 @al_date=al_date from tbl_AssetsLiabilities order by al_date desc
		end
select al_id,al_particulars,al_date,al_col1,al_col2,al_col3,al_col4,batchid,al_createddate from [dbo].[tbl_AssetsLiabilities] 
where al_date=@al_date
commit TRAN GetAssets
END TRY
BEGIN CATCH
RollBack TRAN GetAssets
 SET @OutcomeId = ERROR_NUMBER()  SET @OutcomeDetail = ERROR_MESSAGE()
 END CATCH
  SELECT @OutcomeId AS OutcomeId, @OutcomeDetail AS OutcomeDetail
end


if @operationType='DeleteDoc'
begin
  begin TRY
begin TRAN DeleteDoc
SET @OutcomeId = 1 SET @OutcomeDetail ='Document Deleted Successfully!'
delete from [dbo].[tbl_ALUpload] where al_id=@al_id

delete from tbl_AssetsLiabilities where batchid = @batchid

commit TRAN DeleteDoc
END TRY
BEGIN CATCH
RollBack TRAN DeleteDoc
 SET @OutcomeId = ERROR_NUMBER()  SET @OutcomeDetail = ERROR_MESSAGE()
 END CATCH
  SELECT @OutcomeId AS OutcomeId, @OutcomeDetail AS OutcomeDetail
    SELECT @OutcomeId AS OutcomeId, @OutcomeDetail AS OutcomeDetail

end


end
GO


