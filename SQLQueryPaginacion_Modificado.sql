USE [CursosOnline]
GO
/****** Object:  StoredProcedure [dbo].[Obtener_PaginacionCurso]    Script Date: 13/06/2022 00:16:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[Obtener_PaginacionCurso]
@NombreCurso nvarchar(500),
@Ordenamiento nvarchar(500),
@NumeroPagina int,
@CantidadElementos int,
@TotalRecords int output,
@TotalPaginas int output
as
begin
	set nocount on
	set transaction isolation level read uncommitted
	
	declare @Inicio int
	declare @Fin int

	if @NumeroPagina > 0
		begin
			set @Inicio = (@NumeroPagina * @CantidadElementos) - @CantidadElementos
			set @Fin = @NumeroPagina * @CantidadElementos
		end
	--else
	--	begin
	--		set @Inicio = ((@NumeroPagina * @CantidadElementos) - @CantidadElementos) + 1
	--		set @Fin = @NumeroPagina * @CantidadElementos
	--	end
	
	create table #TMP(
		rowNumber int Identity(1,1),
		Id uniqueidentifier
	)

	declare @SQL nvarchar(max)
	set @SQL='select CursoId from Curso'

	if @NombreCurso is not null
		begin
			set @SQL=@SQL + ' where Titulo Like ''%' + @NombreCurso + '%'' '
		end
	if @Ordenamiento is not null
		begin
			set @SQL=@SQL + ' Order By ' + @Ordenamiento
		end

	insert into #TMP(Id)
	exec sp_executesql @SQL

	select @TotalRecords = COUNT(*) from #TMP

	if @TotalRecords > @CantidadElementos
		begin
			set @TotalPaginas = @TotalRecords / @CantidadElementos
			if (@TotalRecords % @CantidadElementos)> 0
				begin
					set @TotalPaginas = @TotalPaginas + 1
				end
		end
	else
		begin
			set @TotalPaginas = 1
		end

	select c.CursoId,c.Titulo,c.Descripcion,c.FechaPublicacion,c.FotoPortada,c.FechaCreacion,p.PrecioActual,p.Promocion
	from #TMP t inner join Curso c
	on t.Id = c.CursoId left join Precio p
	on c.CursoId = p.CursoId
	Order by c.CursoId
	OFFSET @Inicio  ROWS FETCH NEXT @CantidadElementos ROWS ONLY
end