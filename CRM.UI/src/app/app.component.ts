import { Component, TemplateRef, ViewChild } from '@angular/core';
import { OperadorService } from './services/operador.service';
import { MatDialog } from '@angular/material/dialog';
import { OperadorInt } from './interface/operador.interface';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'CRM.UI';
  operadores: OperadorInt[] = [];
  header: string[] = [];
  operadorSelecionado?: OperadorInt;

  constructor(
    private operadorService: OperadorService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    const columnsToShow = ["nome", "cpf", "email"];
    this.operadorService.getOperadores().subscribe((operadores: OperadorInt[]) => {
      this.operadores = operadores;
      this.header = Object.keys(operadores[0]).filter((header) => {
        return columnsToShow.includes(header);
      })
    });
  }

  initNewOperador() {
    this.operadorSelecionado = {} as OperadorInt;
  }

  editOperador(operador: OperadorInt) {
    this.operadorSelecionado = operador;
  }
}
