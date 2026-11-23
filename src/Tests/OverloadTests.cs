// The TextWriter, Stream and path overloads must produce the same output as the
// snapshotted string/byte[] overloads, so they are compared against those rather
// than given snapshots of their own.
public class OverloadTests
{
    const int startYear = 2025;

    static readonly State[] multipleStates = [State.NSW, State.VIC];

    static async Task<string> Write(Func<TextWriter, Task> export)
    {
        await using var writer = new StringWriter();
        await export(writer);
        return writer.ToString();
    }

    static async Task<byte[]> Write(Func<Stream, Task> export)
    {
        using var stream = new MemoryStream();
        await export(stream);
        return stream.ToArray();
    }

    static async Task<string> WriteFile(Func<string, Task> export)
    {
        using var path = new TempFile();
        await export(path);
        return await File.ReadAllTextAsync(path);
    }

    static async Task<byte[]> WriteFileBytes(Func<string, Task> export)
    {
        using var path = new TempFile();
        await export(path);
        return await File.ReadAllBytesAsync(path);
    }

    static async Task AssertBytes(byte[] actual, byte[] expected) =>
        await Assert.That(actual.SequenceEqual(expected)).IsTrue();

    [Test]
    public async Task HolidaysWriter()
    {
        await Assert.That(await Write(_ => Holidays.ExportToCsv(_, startYear))).IsEqualTo(await Holidays.ExportToCsv(startYear));
        await Assert.That(await Write(_ => Holidays.ExportToCsv(_, State.NSW, startYear))).IsEqualTo(await Holidays.ExportToCsv(State.NSW, startYear));
        await Assert.That(await Write(_ => Holidays.ExportToCsv(_, multipleStates, startYear))).IsEqualTo(await Holidays.ExportToCsv(multipleStates, startYear));

        await Assert.That(await Write(_ => Holidays.ExportToIcs(_, startYear))).IsEqualTo(await Holidays.ExportToIcs(startYear));
        await Assert.That(await Write(_ => Holidays.ExportToIcs(_, State.NSW, startYear))).IsEqualTo(await Holidays.ExportToIcs(State.NSW, startYear));
        await Assert.That(await Write(_ => Holidays.ExportToIcs(_, multipleStates, startYear))).IsEqualTo(await Holidays.ExportToIcs(multipleStates, startYear));

        await Assert.That(await Write(_ => Holidays.ExportToJson(_, startYear))).IsEqualTo(await Holidays.ExportToJson(startYear));
        await Assert.That(await Write(_ => Holidays.ExportToJson(_, State.NSW, startYear))).IsEqualTo(await Holidays.ExportToJson(State.NSW, startYear));
        await Assert.That(await Write(_ => Holidays.ExportToJson(_, multipleStates, startYear))).IsEqualTo(await Holidays.ExportToJson(multipleStates, startYear));

        await Assert.That(await Write(_ => Holidays.ExportToMarkdown(_, startYear))).IsEqualTo(await Holidays.ExportToMarkdown(startYear));
        await Assert.That(await Write(_ => Holidays.ExportToMarkdown(_, State.NSW, startYear))).IsEqualTo(await Holidays.ExportToMarkdown(State.NSW, startYear));
        await Assert.That(await Write(_ => Holidays.ExportToMarkdown(_, multipleStates, startYear))).IsEqualTo(await Holidays.ExportToMarkdown(multipleStates, startYear));

        await Assert.That(await Write(_ => Holidays.ExportToXml(_, startYear))).IsEqualTo(await Holidays.ExportToXml(startYear));
        await Assert.That(await Write(_ => Holidays.ExportToXml(_, State.NSW, startYear))).IsEqualTo(await Holidays.ExportToXml(State.NSW, startYear));
        await Assert.That(await Write(_ => Holidays.ExportToXml(_, multipleStates, startYear))).IsEqualTo(await Holidays.ExportToXml(multipleStates, startYear));
    }

    [Test]
    public async Task HolidaysStream()
    {
        await AssertBytes(await Write(_ => Holidays.ExportToExcel(_, startYear)), await Holidays.ExportToExcel(startYear));
        await AssertBytes(await Write(_ => Holidays.ExportToExcel(_, State.NSW, startYear)), await Holidays.ExportToExcel(State.NSW, startYear));
        await AssertBytes(await Write(_ => Holidays.ExportToExcel(_, multipleStates, startYear)), await Holidays.ExportToExcel(multipleStates, startYear));
    }

    [Test]
    public async Task SchoolHolidaysWriter()
    {
        await Assert.That(await Write(_ => SchoolHolidays.ExportToCsv(_, startYear))).IsEqualTo(await SchoolHolidays.ExportToCsv(startYear));
        await Assert.That(await Write(_ => SchoolHolidays.ExportToCsv(_, State.NSW, startYear))).IsEqualTo(await SchoolHolidays.ExportToCsv(State.NSW, startYear));
        await Assert.That(await Write(_ => SchoolHolidays.ExportToCsv(_, multipleStates, startYear))).IsEqualTo(await SchoolHolidays.ExportToCsv(multipleStates, startYear));

        await Assert.That(await Write(_ => SchoolHolidays.ExportToIcs(_, startYear))).IsEqualTo(await SchoolHolidays.ExportToIcs(startYear));
        await Assert.That(await Write(_ => SchoolHolidays.ExportToIcs(_, State.NSW, startYear))).IsEqualTo(await SchoolHolidays.ExportToIcs(State.NSW, startYear));
        await Assert.That(await Write(_ => SchoolHolidays.ExportToIcs(_, multipleStates, startYear))).IsEqualTo(await SchoolHolidays.ExportToIcs(multipleStates, startYear));

        await Assert.That(await Write(_ => SchoolHolidays.ExportToJson(_, startYear))).IsEqualTo(await SchoolHolidays.ExportToJson(startYear));
        await Assert.That(await Write(_ => SchoolHolidays.ExportToJson(_, State.NSW, startYear))).IsEqualTo(await SchoolHolidays.ExportToJson(State.NSW, startYear));
        await Assert.That(await Write(_ => SchoolHolidays.ExportToJson(_, multipleStates, startYear))).IsEqualTo(await SchoolHolidays.ExportToJson(multipleStates, startYear));

        await Assert.That(await Write(_ => SchoolHolidays.ExportToMarkdown(_, startYear))).IsEqualTo(await SchoolHolidays.ExportToMarkdown(startYear));
        await Assert.That(await Write(_ => SchoolHolidays.ExportToMarkdown(_, State.NSW, startYear))).IsEqualTo(await SchoolHolidays.ExportToMarkdown(State.NSW, startYear));
        await Assert.That(await Write(_ => SchoolHolidays.ExportToMarkdown(_, multipleStates, startYear))).IsEqualTo(await SchoolHolidays.ExportToMarkdown(multipleStates, startYear));

        await Assert.That(await Write(_ => SchoolHolidays.ExportToXml(_, startYear))).IsEqualTo(await SchoolHolidays.ExportToXml(startYear));
        await Assert.That(await Write(_ => SchoolHolidays.ExportToXml(_, State.NSW, startYear))).IsEqualTo(await SchoolHolidays.ExportToXml(State.NSW, startYear));
        await Assert.That(await Write(_ => SchoolHolidays.ExportToXml(_, multipleStates, startYear))).IsEqualTo(await SchoolHolidays.ExportToXml(multipleStates, startYear));
    }

    [Test]
    public async Task SchoolHolidaysStream()
    {
        await AssertBytes(await Write(_ => SchoolHolidays.ExportToExcel(_, startYear)), await SchoolHolidays.ExportToExcel(startYear));
        await AssertBytes(await Write(_ => SchoolHolidays.ExportToExcel(_, State.NSW, startYear)), await SchoolHolidays.ExportToExcel(State.NSW, startYear));
        await AssertBytes(await Write(_ => SchoolHolidays.ExportToExcel(_, multipleStates, startYear)), await SchoolHolidays.ExportToExcel(multipleStates, startYear));
    }

    [Test]
    public async Task SchoolHolidaysPath()
    {
        await Assert.That(await WriteFile(_ => SchoolHolidays.ExportToCsv(_, State.NSW, startYear))).IsEqualTo(await SchoolHolidays.ExportToCsv(State.NSW, startYear));
        await Assert.That(await WriteFile(_ => SchoolHolidays.ExportToCsv(_, multipleStates, startYear))).IsEqualTo(await SchoolHolidays.ExportToCsv(multipleStates, startYear));

        await Assert.That(await WriteFile(_ => SchoolHolidays.ExportToJson(_, State.NSW, startYear))).IsEqualTo(await SchoolHolidays.ExportToJson(State.NSW, startYear));
        await Assert.That(await WriteFile(_ => SchoolHolidays.ExportToJson(_, multipleStates, startYear))).IsEqualTo(await SchoolHolidays.ExportToJson(multipleStates, startYear));

        await Assert.That(await WriteFile(_ => SchoolHolidays.ExportToXml(_, State.NSW, startYear))).IsEqualTo((await SchoolHolidays.ExportToXml(State.NSW, startYear)).Replace("utf-16", "utf-8"));
        await Assert.That(await WriteFile(_ => SchoolHolidays.ExportToXml(_, multipleStates, startYear))).IsEqualTo((await SchoolHolidays.ExportToXml(multipleStates, startYear)).Replace("utf-16", "utf-8"));

        await AssertBytes(await WriteFileBytes(_ => SchoolHolidays.ExportToExcel(_, State.NSW, startYear)), await SchoolHolidays.ExportToExcel(State.NSW, startYear));
        await AssertBytes(await WriteFileBytes(_ => SchoolHolidays.ExportToExcel(_, multipleStates, startYear)), await SchoolHolidays.ExportToExcel(multipleStates, startYear));
    }
}
