from rdkit import Chem
from rdkit.Chem import AllChem
from rdkit.Chem.Draw import MolDraw2DSVG
import json
from rdkit.Chem.rdchem import RWMol


class GldMolecule:
    def __init__(self, title: str, mol: Chem.Mol, offset_position={"x": 0, "y": 0, "z": 0}) -> None:
        self.title = title
        self.mol = mol
        self.offset_position = offset_position


class GldMoleculeTranslator:
    def dumps(self, gld_molecule: GldMolecule) -> str:
        return json.dumps(gld_molecule, cls=GldMoleculeEncoder)

    def loads(self, string: str) -> GldMolecule:
        dic_json = json.loads(string)
        return self.from_json(dic_json)

    @staticmethod
    def from_json(d: json.decoder.JSONObject) -> GldMolecule:
        print(len(d))
        title: str = d.pop("title")
        atoms_d: dict = d.pop("atoms")
        bonds_d: dict = d.pop("bonds")
        center_d: dict = d.pop("offsetPosition")

        mol: RWMol = Chem.RWMol()

        conf = Chem.Conformer()
        for atom_d in atoms_d:
            atom = Chem.Atom(atom_d["atomicNumber"])
            atom.SetFormalCharge(atom_d["formalCharge"])
            atom_index = mol.AddAtom(atom)
            conf.SetAtomPosition(atom_index, [x for x in atom_d["position"].values()])
            atom = mol.GetAtomWithIdx(atom_index)
            atom.SetIntProp("atom_index", atom_d["atomIndex"])

        for bond_d in bonds_d:
            bond_order = Chem.rdchem.BondType.UNSPECIFIED
            if bond_d["bondOrder"] == 1:
                bond_order = Chem.rdchem.BondType.SINGLE
            if bond_d["bondOrder"] == 2:
                bond_order = Chem.rdchem.BondType.DOUBLE
            if bond_d["bondOrder"] == 3:
                bond_order = Chem.rdchem.BondType.TRIPLE
            bond_index = mol.AddBond(bond_d["beginAtomIndex"], bond_d["endAtomIndex"], order=bond_order) - 1
            mol.GetBondWithIdx(bond_index).SetIntProp("bond_index", bond_d["bondIndex"])
            mol.GetBondWithIdx(bond_index).SetIntProp("begin_atom_id", bond_d["beginAtomIndex"])
            mol.GetBondWithIdx(bond_index).SetIntProp("end_atom_id", bond_d["endAtomIndex"])

        mol.AddConformer(conf)

        gld_mol = GldMolecule(title, mol, center_d)
        return gld_mol


class GldMoleculeEncoder(json.JSONEncoder):
    def default(self, obj) -> dict:
        positions = obj.mol.GetConformer(0).GetPositions()
        atoms = obj.mol.GetAtoms()
        bonds = obj.mol.GetBonds()

        d_atoms = []
        for i in range(obj.mol.GetNumAtoms()):
            try:
                atom_index = atoms[i].GetIntProp("atomIndex")
            except Exception as e:
                atom_index = i
            d_atoms.append({
                "atomicNumber": atoms[i].GetAtomicNum(),
                "atomIndex": atom_index,
                "position": {
                    "x": positions[i][0],
                    "y": positions[i][1],
                    "z": positions[i][2],
                },
                "formalCharge": atoms[i].GetFormalCharge(),
            })

        d_bonds = []
        for i in range(obj.mol.GetNumBonds()):

            try:
                bond_index = bonds[i].GetIntProp("bond_index")
            except Exception as e:
                bond_index = i

            try:
                begin_atom_index = bonds[i].GetIntProp("begin_atom_index")
            except Exception as e:
                begin_atom_index = bonds[i].GetBeginAtomIdx()

            try:
                end_atom_id = bonds[i].GetIntProp("end_atom_id")
            except Exception as e:
                end_atom_id = bonds[i].GetEndAtomIdx()

            d_bonds.append({
                "bondIndex": bond_index,
                "beginAtomIndex": begin_atom_index,
                "endAtomIndex": end_atom_id,
                "bondOrder": int(bonds[i].GetBondTypeAsDouble()),
            })

        result = {
            "title": obj.title,
            "atoms": d_atoms,
            "bonds": d_bonds,
            "offsetPosition": {
                "x": obj.offset_position["x"],
                "y": obj.offset_position["y"],
                "z": obj.offset_position["z"],
            }
        }
        return result

    def with_svg(self, obj) -> dict:
        result = self.default(obj)
        mol = Chem.Mol(obj.mol)
        mol = Chem.RemoveHs(mol)
        AllChem.Compute2DCoords(mol)
        svg = MolDraw2DSVG(200, 200)
        svg.DrawMolecule(obj.mol)
        svg.FinishDrawing(None)
        svg = svg.GetDrawingText(None)
        result["svg"] = svg
        return result
