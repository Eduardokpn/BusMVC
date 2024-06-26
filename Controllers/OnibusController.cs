using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Data.Common;
using BusMVC.Models;

namespace BusMVC.Controllers
{
    public class OnibusController : Controller
    {
        public string uriBase = "http://busApi.somee.com/busapi/OnibusControllerDB/";

        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                string uriComplementar = "GetAll";
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<OnibusViewModel> listaOnibus = await Task.Run(() =>
                        JsonConvert.DeserializeObject<List<OnibusViewModel>>(serialized));
                    return View(listaOnibus);

                }
                else
                    throw new System.Exception(serialized);


            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");


            }




        }

         [HttpPost]
        public async Task<ActionResult> CreateAsync(OnibusViewModel o)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(o));
                content.Headers.ContentType = new MediaTypeHeaderValue("application//json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase, content);
                string serialized = await response.Content.ReadAsStringAsync();
            
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = string.Format("Onibus {0}, Id {!} salvo com sucesso!", o.NomeLinha ,serialized);
                    return RedirectToAction("Index");
                }
                else
                    throw new System.Exception(serialized);


            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Create");


            }
            
            
            }


            [HttpGet]
            public ActionResult Create()
            {
                return View();
            }

            [HttpGet]
            public async Task<ActionResult> DetailsAsync(int? id)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    string token = HttpContext.Session.GetString("SessionTokenUsuarios");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString);

                    string serialized = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        OnibusViewModel p = await Task.Run(() =>
                        JsonConvert.DeserializeObject<OnibusViewModel>(serialized));
                        return View(p);
                    }
                    else
                        throw new System.Exception(serialized);
                }
                catch (System.Exception ex)
                {
                    TempData["MensagemErro"] = ex.Message;
                    return RedirectToAction("Index");
                }
            }

             [HttpGet]
            public async Task<ActionResult> EditAsync(int? id)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    string token = HttpContext.Session.GetString("SessionTokenUsuarios");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString);
                    
                    string serialized = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        OnibusViewModel p = await Task.Run(() =>
                        JsonConvert.DeserializeObject<OnibusViewModel>(serialized));
                        return View(p);
                    }
                    else
                        throw new System.Exception(serialized);
                }
                catch (System.Exception ex)
                {
                    TempData["MensagemErro"] = ex.Message;
                    return RedirectToAction("Index");
                }
            }
            
            [HttpPost]
            public async Task<ActionResult> EditAsync(OnibusViewModel p)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    string token = HttpContext.Session.GetString("SessionTokenUsuarios");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var content = new StringContent(JsonConvert.SerializeObject(p));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    
                    HttpResponseMessage response = await httpClient.PutAsync(uriBase,content);
                    string serialized = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["Mensagem"] = 
                            string.Format("Onibus {0}, Linha {1} atualizado com sucesso!", p.NomeLinha, p.Linha);
                        return RedirectToAction("Index");
                    }
                    else
                        throw new System.Exception(serialized);
                }
                catch (System.Exception ex)
                {
                    TempData["MensagemErro"] = ex.Message;
                    return RedirectToAction("Index");
                }
            }

            [HttpGet]
            public async Task<ActionResult> DeleteAsync(int id)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    string token = HttpContext.Session.GetString("SessionTokenUsuarios");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                      
                    HttpResponseMessage response = await httpClient.DeleteAsync(uriBase + id.ToString());
                    string serialized = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["Mensagem"] = string.Format("Onibus {0}, Removido com sucesso", id);
                        return RedirectToAction("Index");
                    }
                    else
                        throw new System.Exception(serialized);
                }
                catch (System.Exception ex)
                {
                    TempData["MensagemErro"] = ex.Message;
                    return RedirectToAction("Index");
                }
            }

    }
}