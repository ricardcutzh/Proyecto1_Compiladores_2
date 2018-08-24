/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Traductor;

/**
 *
 * @author ricar
 */
import ASTTree.ASTNode;
import ManejoError.TError;
import java.util.ArrayList;
public class Opciones {
    TablaOpciones opcs;
    ASTNode raiz;
    ArrayList<TError> errores;
    public Opciones(ASTNode raiz, ArrayList<TError> errores)
    {
        this.raiz = raiz;
        this.opcs = new TablaOpciones();
        this.errores = errores;
    }
    
    private Object obtenerOpciones(ASTNode raiz)
    {
        switch(raiz.getEtiqueta())
        {
            case "INICIO":
            {
                if(raiz.contarHijos()==1)
                {
                    obtenerOpciones(raiz.getHijo(0));
                }
                break;
            }
            case "OPCIONES":
            {
                if(raiz.contarHijos()==1)
                {
                    obtenerOpciones(raiz.getHijo(0));
                }
                break;
            }
            case "L_OPCS":
            {
                if(raiz.contarHijos()==2)
                {
                    obtenerOpciones(raiz.getHijo(0));
                    obtenerOpciones(raiz.getHijo(1));
                    
                }
                break;
            }
            case "OPCS":
            {
                if(raiz.contarHijos()==2)
                {
                    String idlista = raiz.getHijo(0).getEtiqueta();
                    ListaOpciones ls = (ListaOpciones)obtenerOpciones(raiz.getHijo(1));
                    //AQUI YA TENGO LAS LISTA CON SU ID AHORA SI LAS AGREGO
                    if(ls!=null)
                    {
                        ls.setNombreLista(idlista);
                        this.opcs.addListado(ls);   
                    }
                }
                break;
            }
            case "L_ENT"://SINTETIZA UNA LISTA
            {
                if(raiz.contarHijos()==2)
                {
                    Opcion op = (Opcion)obtenerOpciones(raiz.getHijo(0));
                    ListaOpciones ls = (ListaOpciones)obtenerOpciones(raiz.getHijo(1));
                    if(ls == null)
                    {
                        ls = new ListaOpciones("TEMP");
                    }
                    if(!ls.addOpcion(op))
                    {
                        this.errores.add(new TError("Semantico", "Opciones con ID repetidos: "+op.getEtiqueta(), "Nombre", "Opciones"));
                    }
                    return ls;
                }
                break;
            }
            case "ENT"://SINTETIZA UNA OPCION
            {
                if(raiz.contarHijos()==3)
                {
                    String id = raiz.getHijo(0).getEtiqueta();
                    String etiqueta = raiz.getHijo(1).getEtiqueta();
                    String mult = raiz.getHijo(2).getEtiqueta();
                    Opcion op;
                    if(mult.equals("NULL"))
                    {
                        op = new Opcion(id, etiqueta);
                    }
                    else
                    {
                        op = new Opcion(id, etiqueta, mult);
                    }
                    return op;
                }
                break;
            }
        }
        return null;
    }

    public TablaOpciones obteOpciones()
    {
        obtenerOpciones(this.raiz);
        return this.opcs;
    }
    
    
}
