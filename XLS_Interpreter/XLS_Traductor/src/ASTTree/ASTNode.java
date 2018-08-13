/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ASTTree;

/**
 *
 * @author ricar
 */
import java.util.ArrayList;
public class ASTNode{
    
     /*
    * ESTA CLASE ES PARA LA CREACION DEL ARBOL DE ANALISIS SINTACTICO
    */
    String cadenaDot;
    int idnodo;
    int line;
    int column;
    String etiqueta;
    ArrayList<ASTNode> hijos;
    
    public ASTNode(int idnodo, int line, int column, String etiqueta)
    {
        this.idnodo = idnodo;
        this.line = line;
        this.column = column;
        this.etiqueta = etiqueta;
        this.hijos = new ArrayList<>();
    }
    
    public ASTNode(int idnodo, String etiqueta)
    {
        this.idnodo = idnodo;
        this.etiqueta = etiqueta;
        this.hijos = new ArrayList<>();
    }

    public int getIdnodo() 
    {
        return idnodo;
    }

    public int getLine() 
    {
        return line;
    }

    public int getColumn() 
    {
        return column;
    }

    public String getEtiqueta() 
    {
        return etiqueta;
    }
    
    public void addHijo(ASTNode hijo)
    {
        this.hijos.add(hijo);
    }
    
    public ASTNode getHijo(int index)
    {
        return this.hijos.get(index);
    }
    
    public String graficaAST(ASTNode raiz)
    {
        this.cadenaDot = "digraph AST{\n node [shape=box];\n";
        recorreAST(raiz);
        this.cadenaDot += "}";
        return this.cadenaDot;
    }
    
    public int contarHijos()
    {
        return this.hijos.size();
    }
    
    private void recorreAST(ASTNode raiz)
    {
        if(raiz!=null )
        {
            this.cadenaDot += "node_"+raiz.idnodo+"[label= \""+raiz.etiqueta+"\"];\n";
            if(raiz.contarHijos()>0)
            {
                for(int i = 0; i<raiz.hijos.size();i++)
                {
                    this.cadenaDot += "node_"+raiz.idnodo +"->node_"+raiz.getHijo(i).idnodo+";\n";
                    recorreAST(raiz.getHijo(i));
                }
            }
        }
    }
    
}
