using API.Models.DTOs;
using API.Models.Entities;
using API.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ResenasService
    {
        public ResenasService(Repository<Resena> resenaRepo, Repository<Usuario> usuarioRepo, Repository<Reaccionresena> reaccionRepo, IMapper mapper)
        {
            ResenaRepo = resenaRepo;
            UsuarioRepo = usuarioRepo;
            ReaccionRepo = reaccionRepo;
            Mapper = mapper;
        }

        public Repository<Resena> ResenaRepo { get; }
        public Repository<Usuario> UsuarioRepo { get; }
        public Repository<Reaccionresena> ReaccionRepo { get; }
        public IMapper Mapper { get; }


        public List<ResenaDTO> ObtenerResenas(int usuarioId)
        {
            var resenas = ResenaRepo.Query()
                .Include(x => x.Usuario)
                .Include(x => x.Reaccionresenas)
                .Where(x => x.UsuarioId != usuarioId)
                .OrderByDescending(x => x.FechaCreacion)
                .ToList();

            var listaresenas = Mapper.Map<List<ResenaDTO>>(resenas);

            foreach (var r in listaresenas)
            {
                r.MiReaccion = resenas.First(x => x.Id == r.Id).Reaccionresenas
                    .FirstOrDefault(x => x.UsuarioId == usuarioId)?.Tipo ?? 0;
            }

            return listaresenas;
        }

        public List<ResenaDTO> ObtenerMisResenas(int usuarioId)
        {
            var resenas = ResenaRepo.Query()
                 .Include(x => x.Usuario)
                 .Include(x => x.Reaccionresenas)
                 .Where(x => x.UsuarioId == usuarioId)
                 .OrderByDescending(x => x.FechaCreacion)
                 .ToList();

            var listaresenas = Mapper.Map<List<ResenaDTO>>(resenas);

            foreach (var r in listaresenas)
            {
                r.MiReaccion = resenas.First(x => x.Id == r.Id).Reaccionresenas
                    .FirstOrDefault(x => x.UsuarioId == usuarioId)?.Tipo ?? 0;
            }

            return listaresenas;
        }

        public void CrearResena(CrearResenaDTO dto, int usuarioId, string uploadsPath)
        {
            var usuario = UsuarioRepo.Get(usuarioId);
            if (usuario == null)
                throw new NullReferenceException("Usuario no encontrado.");

            var resena = Mapper.Map<Resena>(dto);
            resena.UsuarioId = usuarioId;
            resena.FechaCreacion = DateTime.Now;
            ResenaRepo.Insert(resena);

            GuardarImagen(resena.Id, dto.ImagenBase64 ?? "", uploadsPath);

            var resenaCreada = ResenaRepo.Query().Include(x => x.Usuario).FirstOrDefault(x => x.Id == resena.Id);

        }

        public void EditarResena(EditarResenaDTO dto, int usuarioId, string uploadsPath)
        {
            var resena = ResenaRepo.Get(dto.Id);
            if (resena == null)
                throw new NullReferenceException("Resena no encontrada.");

            if (resena.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("No tienes permiso para editar esta reseña.");

            Mapper.Map(dto, resena);

            if (!string.IsNullOrWhiteSpace(dto.ImagenBase64))
                GuardarImagen(resena.Id, dto.ImagenBase64, uploadsPath);

            ResenaRepo.Update(resena);
        }

        public void EliminarResena(int id, int usuarioId, string uploadsPath)
        {
            var resena = ResenaRepo.Get(id);
            if (resena == null)
                throw new NullReferenceException("Resena no encontrada.");
            if (resena.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("No tienes permiso para eliminar esta reseña.");


            var nombreArchivo = $"{resena.Id}.jpg";
            var rutaArchivo = Path.Combine(uploadsPath, nombreArchivo);
            if (File.Exists(rutaArchivo))
                File.Delete(rutaArchivo);

            ResenaRepo.Delete(id);

        }

        public void Like(int id, int usuarioId)
        {
            Reaccionar(id, usuarioId, 1);
        }

        public void Dislike(int id, int usuarioId)
        {
            Reaccionar(id, usuarioId, 2);
        }

        private void Reaccionar(int id, int usuarioId, int tipo)
        {
            var resena = ResenaRepo.Get(id);
            if (resena == null)
                throw new NullReferenceException("Resena no encontrada.");

            var reaccion = ReaccionRepo.Query()
                .FirstOrDefault(x => x.ResenaId == id && x.UsuarioId == usuarioId);

            if (reaccion == null)
            {
                reaccion = new Reaccionresena
                {
                    ResenaId = id,
                    UsuarioId = usuarioId,
                    Tipo = tipo
                };

                if (tipo == 1)
                    resena.Likes++;
                else
                    resena.Dislikes++;

                ReaccionRepo.Insert(reaccion);
                ResenaRepo.Update(resena);
                return;
            }

            if (reaccion.Tipo == tipo)
            {
                if (tipo == 1)
                    resena.Likes--;
                else
                    resena.Dislikes--;

                ReaccionRepo.Delete(reaccion.Id);
                ResenaRepo.Update(resena);
                return;
            }

            reaccion.Tipo = tipo;

            if (tipo == 1)
            {
                resena.Dislikes--;
                resena.Likes++;
            }
            else
            {
                resena.Likes--;
                resena.Dislikes++;
            }

            ReaccionRepo.Update(reaccion);
            ResenaRepo.Update(resena);
        }

        private void GuardarImagen(int id, string imagenBase64, string uploadsPath)
        {
            var nombreArchivo = $"{id}.jpg";
            Directory.CreateDirectory(uploadsPath);
            var rutaArchivo = Path.Combine(uploadsPath, nombreArchivo);
            var imageBytes = Convert.FromBase64String(imagenBase64);
            File.WriteAllBytes(rutaArchivo, imageBytes);
        }
    }
}
