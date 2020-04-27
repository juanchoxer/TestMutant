using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebApi.DataAccess;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class MutantController : ApiController
    {
        TesterDNA _testerDNA = new TesterDNA();

        [HttpPost]
        public async System.Threading.Tasks.Task<IHttpActionResult> IsMutantAsync([FromBody] ReceiverDNA receiverDNA)
        {
            IHttpActionResult result;
            

            bool isValidDNA = _testerDNA.ValidateDNAAndBuildGrid(receiverDNA.dna);

            if (isValidDNA)
            {
                bool isMutant = _testerDNA.IsMutant(receiverDNA.dna);

                if (isMutant)
                {
                    result = Content(HttpStatusCode.OK, "Mutant");
                }
                else
                {
                    result = Content(HttpStatusCode.Forbidden, "Not a mutant");
                }
                
                string fullDNA = string.Join("|", receiverDNA.dna);

                Mutant mutant = new Mutant() { dna = fullDNA, isMutant = isMutant };

                new Database().AddMutant(mutant);
                
            }
            else
            {
                result = Content(HttpStatusCode.Conflict, "The dna is not valid");
            }

            return result;
        }
    }

    public class ReceiverDNA
    {
        public string[] dna;
    }

    public class TesterDNA
    {
        private readonly char[] _validLetters = { 'A', 'C', 'G', 'T' };
        private readonly int _posibleDirections = 4;
        private readonly int[] _dirRow = { 0, 1, 1, 1 };
        private readonly int[] _dirCol = { 1, -1, 0, 1 };

        private readonly int _neededForMatch = 4;

        private char[,] _grid = null;
        private int _maxRow;
        private int _maxCol;


        internal bool IsMutant(string[] dna)
        {
            bool isMutant = FindMatches();
            return isMutant;
        }

        public bool ValidateDNAAndBuildGrid(string[] dna)
        {
            bool isValidDNA = true;

            _maxRow = dna.Length;
            _maxCol = dna[0].Length;
            _grid = new char[_maxRow, _maxCol];

            int i = 0;
            do
            {
                char[] lettersDNA = dna[i].ToCharArray();

                int j = 0;
                do
                {
                    isValidDNA = _validLetters.Contains(lettersDNA[j]);

                    _grid[i, j] = lettersDNA[j];
                    j++;

                } while (j < lettersDNA.Length && isValidDNA);

                i++;
            } while (i < _maxRow && isValidDNA);

            return isValidDNA;
        }

        public bool FindMatches()
        {
            Pointer pointer = new Pointer() { col = 0, row = 0 };

            int matches = 0;
            bool isEndGrid;
            char letter;

            do
            {
                letter = ReadLetter(pointer);
                matches = matches + CheckMatches(letter, pointer);
                MovePointer(pointer);
                isEndGrid = IsEndGrid(pointer);

            } while (matches < 2 && !isEndGrid);

            return matches > 1;
        }

        private void MovePointer(Pointer pointer)
        {
            if (pointer.col < _maxCol - 1)
            {
                pointer.col++;
            }
            else
            {
                pointer.col = 0;
                pointer.row++;
            }
        }

        private bool IsEndGrid(Pointer pointer)
        {
            return (pointer.row == _maxRow);
        }

        private char ReadLetter(Pointer pointer)
        {
            char letter = ' ';

            if (pointer.row >= 0 && pointer.row < _maxRow && pointer.col >= 0 && pointer.col < _maxCol)
            {
                letter = _grid[pointer.row, pointer.col];
            }

            return letter;
        }

        private int CheckMatches(char letter, Pointer pointer)
        {
            int matches = 0;

            for (int direction = 0; direction < _posibleDirections; direction++)
            {
                if (DirectionIsAMatch(pointer, letter, direction))
                    matches++;
            }
            return matches;
        }

        private bool DirectionIsAMatch(Pointer pointer, char letter, int direction)
        {
            Pointer comparatorPointer = new Pointer() { row = pointer.row, col = pointer.col };
            int matchedLetters = 1;
            bool posibleMatch = true;
            char lettercomparator;

            do
            {
                MovePointerInDirection(comparatorPointer, direction);
                lettercomparator = ReadLetter(comparatorPointer);
                if (letter == lettercomparator)
                {
                    matchedLetters++;
                }
                else
                {
                    posibleMatch = false;
                }

            } while (posibleMatch && matchedLetters < _neededForMatch);

            return matchedLetters == _neededForMatch;
        }

        private void MovePointerInDirection(Pointer comparatorPointer, int direction)
        {
            comparatorPointer.row = comparatorPointer.row + _dirRow[direction];
            comparatorPointer.col = comparatorPointer.col + _dirCol[direction];
        }

    }

    internal class Pointer
    {
        public int row;
        public int col;
    }
}