--SELECT * from DISCOS

--SELECT Id, Descripcion from ESTILOS

--SELECT Id, Descripcion from TIPOSEDICION

--SELECT D.Titulo, D.FechaLanzamiento, D.CantidadCanciones, D.UrlImagenTapa, E.Descripcion as Genero, T.Descripcion as Formato FROM DISCOS D, ESTILOS E, TIPOSEDICION T where E.Id = D.IdEstilo and T.Id = D.IdTipoEdicion;

--UPDATE DISCOS set UrlImagenTapa = 'https://bmusic.cl/cdn/shop/files/siempreeshoy_bmusic_front.jpg?v=1686340570' where id = 2;

--https://www.cmtv.com.ar/tapas-cd/ceratisiempre.jpg

--SELECT D.Titulo, D.FechaLanzamiento, D.CantidadCanciones, D.UrlImagenTapa, D.IdEstilo, D.IdTipoEdicion, E.Descripcion as Genero, T.Descripcion as Formato FROM DISCOS D, ESTILOS E, TIPOSEDICION T where E.Id = D.IdEstilo and T.Id = D.IdTipoEdicion;

--UPDATE DISCOS set UrlImagenTapa = ' ' where Id = 7 

--Insert into DISCOS (Titulo, FechaLanzamiento, CantidadCanciones) values ('', '', 1)
--DELETE DISCOS where id = 4

/*INSERT INTO DISCOS (Titulo, FechaLanzamiento, CantidadCanciones, UrlImagenTapa, IdEstilo, IdTipoEdicion)
VALUES ('Oktubre', '1986-11-04', 9, 'https://cocacola634974865.files.wordpress.com/2023/02/0-tapa-para-youtube.jpg?w=800&h=800&crop=1', 3, 2);*/

--UPDATE DISCOS set Titulo = '', FechaLanzamiento = '', CantidadCanciones = 1, UrlImagenTapa = '', IdEstilo = 1, IdTipoEdicion = 1 

--UPDATE DISCOS set  Activo= 1;

