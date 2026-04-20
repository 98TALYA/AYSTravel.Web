using AYSTravel.Data;
using AYSTravel.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AYSTravel.Logic
{
    public class ActiviteManager
    {
        private readonly ApplicationDbContext _context;
        public ActiviteManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Activite> GetAll()
        {
            return _context.Activites.ToList();
        }

        public Activite GetById(int id)
        {
            return _context.Activites.FirstOrDefault(a => a.Id == id);
        }

        public void Add(Activite activite)
        {
            _context.Activites.Add(activite);
            _context.SaveChanges();
        }

        /* 
         Plan (pseudocode détaillé) :
         - Recevoir une instance `activite` contenant les nouvelles valeurs.
         - Récupérer l'entité existante depuis le contexte via Find(activite.Id).
         - Si l'entité existe :
           - Sauvegarder la valeur courante de `ImageUrl` de l'entité existante.
           - Copier toutes les valeurs de `activite` vers l'entité existante en utilisant CurrentValues.SetValues pour éviter d'énumérer chaque propriété.
           - Si la nouvelle `activite.ImageUrl` est vide ou nulle => restaurer l'ancienne `ImageUrl` pour ne pas l'écraser.
           - Appeler SaveChanges().
         - Si l'entité n'existe pas => ne rien faire.
        */

        // -------------------------
        // UPDATE pour Activite
        // -------------------------
        public void Update(Activite activite)
        {
            var existing = _context.Activites.Find(activite.Id);

            if (existing != null)
            {
                // Préserver l'image actuelle
                var existingImage = existing.ImageUrl;

                // Copier toutes les valeurs de activite vers existing
                _context.Entry(existing).CurrentValues.SetValues(activite);

                // Ne pas écraser l'image si la nouvelle valeur est vide ou nulle
                if (string.IsNullOrEmpty(activite.ImageUrl))
                {
                    existing.ImageUrl = existingImage;
                }

                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var activite = GetById(id);
            if (activite != null)
            {
                _context.Activites.Remove(activite);
                _context.SaveChanges();
            }
        }
    }
}