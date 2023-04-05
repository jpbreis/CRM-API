import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Operador } from '../../../interface/operador.interface';
import { OperadorService } from '../../../services/operador.service';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap'

@Component({
  selector: 'app-add-operador',
  templateUrl: './add-operador.component.html',
  styleUrls: ['./add-operador.component.css']
})
export class AddOperadorComponent implements OnInit {
  @Input() operador?: Operador;
  @Output() operadoresUpdate = new EventEmitter<Operador[]>();

  constructor(private operadorService: OperadorService, modalService: NgbModal) {
  }

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  updateOperador(operador: Operador) {
    this.operadorService
    .updateOperador(operador)
    .subscribe((operadores: Operador[]) => this.operadoresUpdate.emit(operadores));
  }

  createOperador(operador: Operador) {
    this.operadorService
    .createOperador(operador)
    .subscribe((operadores: Operador[]) => this.operadoresUpdate.emit(operadores));
  }
}
