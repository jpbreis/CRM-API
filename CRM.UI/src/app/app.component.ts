import { Component, TemplateRef, ViewChild } from '@angular/core';
import { OperadorService } from './services/operador.service';
import { MatDialog } from '@angular/material/dialog';
import { Operador } from './interface/operador.interface';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'CRM.UI';
  operadores: Operador[] = [];
  header: string[] = [];
  operadorSelecionado?: Operador;

  constructor(
    private operadorService: OperadorService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    const columnsToShow = ["nome", "cpf", "email"];
    this.operadorService.getOperadores().subscribe((operadores: Operador[]) => {
      this.operadores = operadores;
      this.header = Object.keys(operadores[0]).filter((header) => {
        return columnsToShow.includes(header);
      })
    });
  }

  initNewOperador() {
    this.operadorSelecionado = {} as Operador;
  }

  editOperador(operador: Operador) {
    this.operadorSelecionado = operador;
  }
}
